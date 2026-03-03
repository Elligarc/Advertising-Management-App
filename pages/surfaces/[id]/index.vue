<template>
  <div class="surface-detail">
    <div class="page-header">
      <NuxtLink to="/surfaces" class="back-link">К списку поверхностей</NuxtLink>
      <h1 class="page-title">{{ surface?.name || 'Поверхность' }}</h1>
      <NuxtLink v-if="surface" :to="`/surfaces/${surface.id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
    </div>

    <div v-if="surface" class="detail-card card">
      <div class="detail-row">
        <span class="label">Адрес</span>
        <span class="value">{{ surface.address }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Тип</span>
        <span class="value">{{ surface.type }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Цена за день</span>
        <span class="value">{{ surface.price }} ₽</span>
      </div>
      <div class="detail-row">
        <span class="label">Статус</span>
        <span :class="['status', surface.statusClass]">{{ surface.statusText }}</span>
      </div>
      <div v-if="surface.currentRental" class="block rental-info">
        <h3>Текущая аренда</h3>
        <p><strong>Клиент:</strong> {{ surface.currentRental.client }}</p>
        <p><strong>Период:</strong> {{ surface.currentRental.startDate }} — {{ surface.currentRental.endDate }}</p>
      </div>
      <div v-if="surface.downtime" class="block downtime-info">
        <h3>Простой</h3>
        <p><strong>Причина:</strong> {{ surface.downtime.reason }}</p>
        <p><strong>Ожидаемое окончание:</strong> {{ surface.downtime.endDate }}</p>
      </div>
      <div class="actions">
        <NuxtLink :to="`/surfaces/${surface.id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
        <button v-if="surface.status === 'free'" class="btn btn-success" @click="navigateTo(`/surfaces/rent/${surface.id}`)">Сдать в аренду</button>
        <button v-if="surface.status === 'free'" class="btn btn-warning" @click="navigateTo(`/downtime/add?surface=${surface.id}`)">Отметить простой</button>
      </div>
    </div>
    <p v-else class="empty">Поверхность не найдена.</p>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const route = useRoute()
const id = computed(() => Number(route.params.id))

const surfaces = ref([
  { id: 1, name: 'Билборд Тверская 15', address: 'г. Москва, ул. Тверская, д.15', type: 'Билборд 3x6', price: 5000, status: 'busy', statusClass: 'status-busy', statusText: 'Занята', currentRental: { client: 'ООО Ромашка', startDate: '01.03.2026', endDate: '15.03.2026' } },
  { id: 2, name: 'Ситилайт Арбат 10', address: 'г. Москва, ул. Арбат, д.10', type: 'Ситилайт 1.2x1.8', price: 3000, status: 'repair', statusClass: 'status-repair', statusText: 'Простой', downtime: { reason: 'Замена подсветки', endDate: '10.03.2026' } },
  { id: 3, name: 'Видеоэкран Садовая 5', address: 'г. Москва, ул. Садовая, д.5', type: 'Видеоэкран', price: 8000, status: 'free', statusClass: 'status-free', statusText: 'Свободна' },
  { id: 4, name: 'Билборд Ленинский 20', address: 'г. Москва, Ленинский пр-т, д.20', type: 'Билборд 3x6', price: 5500, status: 'busy', statusClass: 'status-busy', statusText: 'Занята', currentRental: { client: 'ТехноПлюс', startDate: '01.03.2026', endDate: '10.04.2026' } },
  { id: 5, name: 'Баннер МКАД 45 км', address: 'МКАД, 45-й км, внешняя сторона', type: 'Баннер 3x12', price: 7000, status: 'free', statusClass: 'status-free', statusText: 'Свободна' }
])

const surface = computed(() => surfaces.value.find(s => s.id === id.value))
</script>

<style scoped>
.surface-detail { max-width: 720px; }
.back-link { color: #1e3c72; text-decoration: none; font-size: 0.95rem; margin-bottom: 0.5rem; display: inline-block; }
.back-link:hover { text-decoration: underline; }
.page-header { display: flex; flex-wrap: wrap; align-items: center; gap: 1rem; margin-bottom: 1.5rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; flex: 1; }
.detail-card { padding: 1.5rem; }
.detail-row { display: flex; padding: 0.75rem 0; border-bottom: 1px solid #e5e7eb; gap: 1rem; }
.detail-row .label { color: #6b7280; min-width: 140px; }
.detail-row .value { color: #1a1a2e; font-weight: 500; }
.block { margin-top: 1.5rem; padding: 1rem; border-radius: 8px; }
.block h3 { font-size: 1rem; margin-bottom: 0.5rem; color: #1a1a2e; }
.rental-info { background: #f3f4f6; }
.downtime-info { background: #fef3c7; }
.actions { display: flex; gap: 0.75rem; margin-top: 1.5rem; flex-wrap: wrap; }
.empty { color: #6b7280; }
</style>
