<template>
  <div class="client-detail">
    <div class="page-header">
      <NuxtLink to="/clients" class="back-link">К списку клиентов</NuxtLink>
      <h1 class="page-title">{{ client?.name || 'Клиент' }}</h1>
      <NuxtLink v-if="client" :to="`/clients/${client.id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
    </div>

    <div v-if="client" class="detail-card card">
      <div class="detail-row">
        <span class="label">Контактное лицо</span>
        <span class="value">{{ client.contactPerson }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Телефон</span>
        <span class="value">{{ client.phone }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Email</span>
        <span class="value">{{ client.email }}</span>
      </div>
      <div class="detail-row">
        <span class="label">ИНН</span>
        <span class="value">{{ client.inn }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Адрес</span>
        <span class="value">{{ client.address }}</span>
      </div>
      <div class="block stats-block">
        <h3>Статистика</h3>
        <p>Активных аренд: <strong>{{ client.activeRentals }}</strong></p>
        <p>Всего аренд: <strong>{{ client.totalRentals }}</strong></p>
        <p>На сумму: <strong>{{ client.totalSpent }} ₽</strong></p>
      </div>
      <div class="actions">
        <NuxtLink :to="`/clients/${client.id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
        <button class="btn btn-success" @click="navigateTo(`/surfaces?rentClient=${client.id}`)">Новая аренда</button>
      </div>
    </div>
    <p v-else class="empty">Клиент не найден.</p>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const route = useRoute()
const id = computed(() => Number(route.params.id))

const clients = ref([
  { id: 1, name: 'ООО "Ромашка"', contactPerson: 'Иванов Иван Иванович', phone: '+7 (999) 123-45-67', email: 'ivan@romashka.ru', inn: '7712345678', address: 'г. Москва, ул. Цветной бульвар, д.10', activeRentals: 3, totalRentals: 12, totalSpent: 1250000 },
  { id: 2, name: 'ООО "ТехноПлюс"', contactPerson: 'Петров Петр Петрович', phone: '+7 (999) 765-43-21', email: 'petrov@techno.ru', inn: '7723456789', address: 'г. Москва, ул. Новый Арбат, д.5', activeRentals: 1, totalRentals: 5, totalSpent: 480000 },
  { id: 3, name: 'ИП Сидоров', contactPerson: 'Сидоров Сидор', phone: '+7 (999) 111-22-33', email: 'sidorov@mail.ru', inn: '7734567890', address: 'г. Москва, ул. Ленина, д.1', activeRentals: 0, totalRentals: 2, totalSpent: 120000 }
])

const client = computed(() => clients.value.find(c => c.id === id.value))
</script>

<style scoped>
.client-detail { max-width: 720px; }
.back-link { color: #1e3c72; text-decoration: none; font-size: 0.95rem; margin-bottom: 0.5rem; display: inline-block; }
.back-link:hover { text-decoration: underline; }
.page-header { display: flex; flex-wrap: wrap; align-items: center; gap: 1rem; margin-bottom: 1.5rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; flex: 1; }
.detail-card { padding: 1.5rem; }
.detail-row { display: flex; padding: 0.75rem 0; border-bottom: 1px solid #e5e7eb; gap: 1rem; }
.detail-row .label { color: #6b7280; min-width: 160px; }
.detail-row .value { color: #1a1a2e; font-weight: 500; }
.block { margin-top: 1.5rem; padding: 1rem; border-radius: 8px; }
.block h3 { font-size: 1rem; margin-bottom: 0.5rem; color: #1a1a2e; }
.stats-block { background: #f3f4f6; }
.actions { display: flex; gap: 0.75rem; margin-top: 1.5rem; flex-wrap: wrap; }
.empty { color: #6b7280; }
</style>
