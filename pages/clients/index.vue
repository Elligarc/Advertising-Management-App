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
    </div>

    <div v-if="pending" class="state-msg">Загрузка...</div>
    <div v-else-if="error" class="state-msg error-msg">Ошибка загрузки клиентов</div>
    <div v-else-if="filteredClients.length === 0" class="state-msg">
      <p>Клиенты не найдены</p>
      <NuxtLink to="/clients/add" class="btn btn-primary">Добавить первого клиента</NuxtLink>
    </div>

    <div v-else class="grid">
      <div v-for="client in filteredClients" :key="client.id" class="client-card card">
        <div class="card-header">
          <h3>{{ client.name || 'Без названия' }}</h3>
        </div>
        <div class="card-body">
          <p class="phone"><span class="label">Телефон:</span> {{ client.phone || 'Не указан' }}</p>
        </div>
        <div class="card-footer">
          <NuxtLink :to="`/clients/${client.id}`" class="btn btn-primary btn-sm">Карточка</NuxtLink>
          <NuxtLink :to="`/clients/${client.id}/edit`" class="btn btn-secondary btn-sm">Редактировать</NuxtLink>
          <button class="btn btn-success btn-sm" @click="navigateTo(`/surfaces?rentClient=${client.id}`)">Новая аренда</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
const API = 'http://localhost:5000'
const searchQuery = ref('')

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
.filters { margin-bottom: 1.5rem; }
.search-input { width: 100%; max-width: 400px; padding: 0.75rem 1rem; border: 2px solid #e5e7eb; border-radius: 8px; font-size: 1rem; }
.search-input:focus { outline: none; border-color: #1e3c72; }
.grid { display: grid; grid-template-columns: repeat(auto-fill, minmax(300px, 1fr)); gap: 1.5rem; }
.client-card { display: flex; flex-direction: column; }
.card-header { padding: 1.5rem 1.5rem 0.5rem; }
.card-header h3 { margin: 0; color: #1a1a2e; font-size: 1.1rem; font-weight: 600; }
.card-body { padding: 0.5rem 1.5rem; flex: 1; }
.card-body p { margin: 0.35rem 0; color: #4b5563; font-size: 0.95rem; }
.label { font-weight: 500; color: #6b7280; margin-right: 0.25rem; }
.card-footer { display: flex; gap: 0.5rem; flex-wrap: wrap; padding: 1rem 1.5rem; border-top: 1px solid #e5e7eb; }
.btn-sm { padding: 0.5rem 1rem; font-size: 0.875rem; }
.state-msg { padding: 2rem; text-align: center; color: #6b7280; }
.error-msg { color: #e53e3e; background: #fff5f5; border-radius: 8px; }
@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
</style>