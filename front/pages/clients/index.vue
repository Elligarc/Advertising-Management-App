<template>
  <div class="clients-page">
    <div class="page-header">
      <h1 class="page-title">Клиенты</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/clients" class="tab" active-class="tab--active">Список</NuxtLink>
      <NuxtLink to="/clients/add" class="tab" active-class="tab--active">Добавить клиента</NuxtLink>
    </nav>

    <div class="filters">
      <input v-model="searchQuery" type="text" placeholder="Поиск по названию или телефону..." class="search-input">
      <div class="view-toggle">
        <button 
          :class="['view-btn', { active: viewMode === 'cards' }]"
          @click="viewMode = 'cards'"
        >
          Карточки
        </button>
        <button 
          :class="['view-btn', { active: viewMode === 'list' }]"
          @click="viewMode = 'list'"
        >
          Список
        </button>
      </div>
    </div>

    <div v-if="pending" class="state-msg">Загрузка...</div>
    <div v-else-if="error" class="state-msg error-msg">Ошибка загрузки клиентов</div>
    <div v-else-if="filteredClients.length === 0" class="state-msg">
      <p>Клиенты не найдены</p>
      <NuxtLink to="/clients/add" class="btn btn-primary">Добавить первого клиента</NuxtLink>
    </div>

    <div v-else>
      <!-- Карточная вьюха -->
      <div v-if="viewMode === 'cards'" class="grid">
        <ClientCard 
          v-for="client in filteredClients" 
          :key="client.id" 
          :client="client"
        />
      </div>
      
      <!-- Списочная вьюха -->
      <div v-else class="list-view">
        <div class="list-header">
          <div class="list-col col-name">Название</div>
          <div class="list-col col-phone">Телефон</div>
          <div class="list-col col-actions">Действия</div>
        </div>
        <div 
          v-for="client in filteredClients" 
          :key="client.id" 
          class="list-row"
        >
          <div class="list-col col-name">{{ client.name || 'Без названия' }}</div>
          <div class="list-col col-phone">{{ client.phone || 'Не указан' }}</div>
          <div class="list-col col-actions">
            <NuxtLink :to="`/clients/${client.id}`" class="btn btn-primary btn-sm">Карточка</NuxtLink>
            <NuxtLink :to="`/clients/${client.id}/edit`" class="btn btn-secondary btn-sm">Редактировать</NuxtLink>
            <button class="btn btn-success btn-sm" @click="navigateTo(`/surfaces?rentClient=${client.id}`)">Новая аренда</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const API = 'http://localhost:5000'
const searchQuery = ref('')
const viewMode = ref('cards') // 'cards' или 'list'

const { data: clients, pending, error } = await useFetch(`${API}/api/Clients`, {
  default: () => []
})

const filteredClients = computed(() => {
  const q = searchQuery.value.toLowerCase().trim()
  if (!q) return clients.value
  return clients.value.filter(c =>
    c.name?.toLowerCase().includes(q) || c.phone?.includes(q)
  )
})
</script>

<style scoped>
.clients-page { animation: fadeIn 0.3s ease; }
.page-header { margin-bottom: 1rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; }
.tabs { display: flex; gap: 0.25rem; margin-bottom: 1.5rem; border-bottom: 2px solid #e5e7eb; }
.tab { padding: 0.75rem 1.25rem; color: #6b7280; text-decoration: none; font-weight: 500; border-bottom: 2px solid transparent; margin-bottom: -2px; transition: all 0.2s; }
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }
.filters { 
  margin-bottom: 1.5rem; 
  display: flex; 
  gap: 1rem; 
  align-items: center; 
  flex-wrap: wrap; 
}
.search-input { 
  width: 100%; 
  max-width: 400px; 
  padding: 0.75rem 1rem; 
  border: 2px solid #e5e7eb; 
  border-radius: 8px; 
  font-size: 1rem; 
}
.search-input:focus { outline: none; border-color: #1e3c72; }

/* Стили для переключателя вида */
.view-toggle {
  display: flex;
  gap: 0.5rem;
  background: #f3f4f6;
  padding: 0.25rem;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
}

.view-btn {
  padding: 0.5rem 1rem;
  border: none;
  background: transparent;
  color: #6b7280;
  font-weight: 500;
  border-radius: 6px;
  cursor: pointer;
  transition: all 0.2s;
  font-size: 0.875rem;
}

.view-btn:hover {
  color: #1e3c72;
  background: #e5e7eb;
}

.view-btn.active {
  background: #1e3c72;
  color: white;
  box-shadow: 0 2px 4px rgba(30, 60, 114, 0.2);
}

/* Сетка для карточек */
.grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; }
.client-card { display: flex; flex-direction: column; }
.card-header { padding: 1.5rem 1.5rem 0.5rem; }
.card-header h3 { margin: 0; color: #1a1a2e; font-size: 1.1rem; font-weight: 600; }
.card-body { padding: 0.5rem 1.5rem; flex: 1; }
.card-body p { margin: 0.35rem 0; color: #4b5563; font-size: 0.95rem; }
.label { font-weight: 500; color: #6b7280; margin-right: 0.25rem; }
.card-footer { display: flex; gap: 0.5rem; flex-wrap: wrap; padding: 1rem 1.5rem; border-top: 1px solid #e5e7eb; }
.btn-sm { padding: 0.5rem 1rem; font-size: 0.875rem; }

/* Списочная вьюха */
.list-view {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  overflow: hidden;
  background: white;
}

.list-header {
  display: grid;
  grid-template-columns: 2fr 1fr 2fr;
  background: #f9fafb;
  border-bottom: 1px solid #e5e7eb;
  font-weight: 600;
  color: #374151;
}

.list-row {
  display: grid;
  grid-template-columns: 2fr 1fr 2fr;
  border-bottom: 1px solid #f3f4f6;
  transition: background-color 0.2s;
}

.list-row:hover {
  background: #f9fafb;
}

.list-row:last-child {
  border-bottom: none;
}

.list-col {
  padding: 1rem 1.5rem;
  display: flex;
  align-items: center;
}

.col-name {
  font-weight: 500;
  color: #1a1a2e;
}

.col-phone {
  color: #6b7280;
}

.col-actions {
  gap: 0.5rem;
  flex-wrap: wrap;
}

/* Адаптивность */
@media (max-width: 768px) {
  .list-header, .list-row {
    grid-template-columns: 1fr;
    gap: 0.5rem;
  }
  
  .list-col {
    border-bottom: 1px solid #e5e7eb;
  }
  
  .list-col:last-child {
    border-bottom: none;
  }
  
  .view-toggle {
    width: 100%;
    justify-content: center;
  }
}

.state-msg { padding: 2rem; text-align: center; color: #6b7280; }
.error-msg { color: #e53e3e; background: #fff5f5; border-radius: 8px; }
@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
</style>
