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
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Поиск по названию или контактам..."
        class="search-input"
      >
    </div>

    <div class="grid">
      <div v-for="client in filteredClients" :key="client.id" class="client-card card">
        <div class="card-header">
          <h3>{{ client.name }}</h3>
          <span class="client-status" :class="client.activeRentals > 0 ? 'active' : 'inactive'">
            {{ client.activeRentals > 0 ? 'Активен' : 'Не активен' }}
          </span>
        </div>
        <div class="card-body">
          <p class="contact">{{ client.contactPerson }}</p>
          <p class="phone">{{ client.phone }}</p>
          <p class="email">{{ client.email }}</p>
          <div class="stats">
            <div class="stat"><span class="stat-label">Активных аренд:</span> <span class="stat-value">{{ client.activeRentals }}</span></div>
            <div class="stat"><span class="stat-label">Всего аренд:</span> <span class="stat-value">{{ client.totalRentals }}</span></div>
            <div class="stat"><span class="stat-label">На сумму:</span> <span class="stat-value">{{ client.totalSpent }} ₽</span></div>
          </div>
        </div>
        <div class="card-footer">
          <NuxtLink :to="`/clients/${client.id}`" class="btn btn-primary btn-sm">Карточка</NuxtLink>
          <NuxtLink :to="`/clients/${client.id}/edit`" class="btn btn-secondary btn-sm">Редактировать</NuxtLink>
          <button class="btn btn-success btn-sm" @click="newRental(client.id)">Новая аренда</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const searchQuery = ref('')

const clients = ref([
  { id: 1, name: 'ООО "Ромашка"', contactPerson: 'Иванов Иван Иванович', phone: '+7 (999) 123-45-67', email: 'ivan@romashka.ru', inn: '7712345678', address: 'г. Москва, ул. Цветной бульвар, д.10', activeRentals: 3, totalRentals: 12, totalSpent: 1250000 },
  { id: 2, name: 'ООО "ТехноПлюс"', contactPerson: 'Петров Петр Петрович', phone: '+7 (999) 765-43-21', email: 'petrov@techno.ru', inn: '7723456789', address: 'г. Москва, ул. Новый Арбат, д.5', activeRentals: 1, totalRentals: 5, totalSpent: 480000 },
  { id: 3, name: 'ИП Сидоров', contactPerson: 'Сидоров Сидор', phone: '+7 (999) 111-22-33', email: 'sidorov@mail.ru', inn: '7734567890', address: 'г. Москва, ул. Ленина, д.1', activeRentals: 0, totalRentals: 2, totalSpent: 120000 }
])

const filteredClients = computed(() => {
  const q = searchQuery.value.toLowerCase()
  if (!q) return clients.value
  return clients.value.filter(c =>
    c.name.toLowerCase().includes(q) ||
    (c.contactPerson && c.contactPerson.toLowerCase().includes(q)) ||
    (c.phone && c.phone.includes(q)) ||
    (c.email && c.email.toLowerCase().includes(q))
  )
})

function newRental(id) {
  navigateTo(`/surfaces?rentClient=${id}`)
}
</script>

<style scoped>
.clients-page { animation: fadeIn 0.3s ease; }
.page-header { margin-bottom: 1rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; }

.tabs {
  display: flex;
  gap: 0.25rem;
  margin-bottom: 1.5rem;
  border-bottom: 2px solid #e5e7eb;
}
.tab {
  padding: 0.75rem 1.25rem;
  color: #6b7280;
  text-decoration: none;
  font-weight: 500;
  border-bottom: 2px solid transparent;
  margin-bottom: -2px;
  transition: color 0.2s, border-color 0.2s;
}
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }

.client-status { padding: 0.25rem 0.75rem; border-radius: 20px; font-size: 0.8rem; font-weight: 600; }
.client-status.active { background: #d1fae5; color: #065f46; }
.client-status.inactive { background: #e5e7eb; color: #6b7280; }
.card-body p { margin: 0.35rem 0; color: #4b5563; font-size: 0.95rem; }
.stats { margin-top: 0.75rem; padding-top: 0.75rem; border-top: 1px solid #e5e7eb; display: flex; flex-wrap: wrap; gap: 1rem; }
.stat-label { color: #6b7280; font-size: 0.85rem; }
.stat-value { font-weight: 600; color: #1a1a2e; }
.card-footer { display: flex; gap: 0.5rem; flex-wrap: wrap; margin-top: 1rem; padding-top: 1rem; border-top: 1px solid #e5e7eb; }
.btn-sm { padding: 0.5rem 1rem; font-size: 0.875rem; }
@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
</style>
