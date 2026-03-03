<template>
  <div class="surfaces-page">
    <div class="page-header">
      <h1 class="page-title">Рекламные поверхности</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/surfaces" class="tab" active-class="tab--active">Список</NuxtLink>
      <NuxtLink to="/surfaces/add" class="tab" active-class="tab--active">Добавить поверхность</NuxtLink>
    </nav>

    <div class="filters">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Поиск по названию или адресу..."
        class="search-input"
      >
      <select v-model="typeFilter" class="select-filter">
        <option value="all">Все типы</option>
        <option value="billboard">Билборд 3x6</option>
        <option value="citylight">Ситилайт</option>
        <option value="video">Видеоэкран</option>
        <option value="banner">Баннер</option>
      </select>
      <select v-model="statusFilter" class="select-filter">
        <option value="all">Все статусы</option>
        <option value="free">Свободны</option>
        <option value="busy">Заняты</option>
        <option value="repair">В простое</option>
      </select>
      <button class="btn btn-secondary" @click="resetFilters">Сбросить</button>

      <div class="view-toggle">
        <button
          type="button"
          class="view-toggle__btn"
          :class="{ 'view-toggle__btn--active': viewMode === 'normal' }"
          @click="viewMode = 'normal'"
        >
          Крупные карточки
        </button>
        <button
          type="button"
          class="view-toggle__btn"
          :class="{ 'view-toggle__btn--active': viewMode === 'compact' }"
          @click="viewMode = 'compact'"
        >
          Компактно
        </button>
      </div>
    </div>

    <div class="grid">
      <div
        v-for="surface in filteredSurfaces"
        :key="surface.id"
        :class="['surface-card', 'card', viewMode === 'compact' ? 'surface-card--compact' : '']"
      >
        <div class="card-header">
          <h3>{{ surface.name }}</h3>
          <span :class="['status', surface.statusClass]">{{ surface.statusText }}</span>
        </div>
        <div class="card-body">
          <p class="address">{{ surface.address }}</p>
          <p class="type">{{ surface.type }}</p>
          <p class="price">{{ surface.price }} ₽/день</p>
          <div v-if="surface.currentRental" class="rental-info">
            <p class="client">{{ surface.currentRental.client }}</p>
            <p class="dates">{{ surface.currentRental.startDate }} — {{ surface.currentRental.endDate }}</p>
          </div>
          <div v-if="surface.downtime" class="downtime-info">
            <p class="reason">{{ surface.downtime.reason }}</p>
            <p class="downtime-dates">до {{ surface.downtime.endDate }}</p>
          </div>
        </div>
        <div class="card-footer">
          <NuxtLink :to="`/surfaces/${surface.id}`" class="btn btn-primary btn-sm">Подробнее</NuxtLink>
          <NuxtLink :to="`/surfaces/${surface.id}/edit`" class="btn btn-secondary btn-sm">Редактировать</NuxtLink>
          <button
            class="btn btn-secondary btn-sm"
            type="button"
            @click="addToContract(surface.id)"
          >
            Добавить в договор
          </button>
          <button class="btn btn-success btn-sm" :disabled="surface.status !== 'free'" @click="rentSurface(surface.id)">
            Сдать в аренду
          </button>
          <button v-if="surface.status === 'free'" class="btn btn-warning btn-sm" @click="markDowntime(surface.id)">
            Отметить простой
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const searchQuery = ref('')
const typeFilter = ref('all')
const statusFilter = ref('all')
const viewMode = ref('normal')

const surfaces = ref([
  { id: 1, name: 'Билборд Тверская 15', address: 'г. Москва, ул. Тверская, д.15', type: 'Билборд 3x6', price: 5000, status: 'busy', statusClass: 'status-busy', statusText: 'Занята', currentRental: { client: 'ООО Ромашка', startDate: '01.03.2026', endDate: '15.03.2026' } },
  { id: 2, name: 'Ситилайт Арбат 10', address: 'г. Москва, ул. Арбат, д.10', type: 'Ситилайт 1.2x1.8', price: 3000, status: 'repair', statusClass: 'status-repair', statusText: 'Простой', downtime: { reason: 'Замена подсветки', endDate: '10.03.2026' } },
  { id: 3, name: 'Видеоэкран Садовая 5', address: 'г. Москва, ул. Садовая, д.5', type: 'Видеоэкран', price: 8000, status: 'free', statusClass: 'status-free', statusText: 'Свободна' },
  { id: 4, name: 'Билборд Ленинский 20', address: 'г. Москва, Ленинский пр-т, д.20', type: 'Билборд 3x6', price: 5500, status: 'busy', statusClass: 'status-busy', statusText: 'Занята', currentRental: { client: 'ТехноПлюс', startDate: '01.03.2026', endDate: '10.04.2026' } },
  { id: 5, name: 'Баннер МКАД 45 км', address: 'МКАД, 45-й км, внешняя сторона', type: 'Баннер 3x12', price: 7000, status: 'free', statusClass: 'status-free', statusText: 'Свободна' }
])

const filteredSurfaces = computed(() => {
  return surfaces.value.filter(s => {
    const q = searchQuery.value.toLowerCase()
    const matchSearch = !q || s.name.toLowerCase().includes(q) || s.address.toLowerCase().includes(q)
    const matchType = typeFilter.value === 'all' || s.type.toLowerCase().includes(typeFilter.value.toLowerCase())
    const matchStatus = statusFilter.value === 'all' || s.status === statusFilter.value
    return matchSearch && matchType && matchStatus
  })
})

function resetFilters() {
  searchQuery.value = ''
  typeFilter.value = 'all'
  statusFilter.value = 'all'
}

function rentSurface(id) {
  navigateTo(`/surfaces/rent/${id}`)
}

function markDowntime(id) {
  navigateTo(`/downtime/add?surface=${id}`)
}

function addToContract(id) {
  navigateTo(`/contracts/add?surface=${id}`)
}
</script>

<style scoped>
.surfaces-page { animation: fadeIn 0.3s ease; }
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

.surface-card { display: flex; flex-direction: column; }
.surface-card--compact {
  padding: 0.75rem 1rem;
}
.surface-card--compact .card-header {
  margin-bottom: 0.5rem;
  padding-bottom: 0.5rem;
}
.surface-card--compact .card-body p {
  font-size: 0.85rem;
  margin: 0.25rem 0;
}
.surface-card--compact .card-footer {
  margin-top: 0.5rem;
  padding-top: 0.5rem;
}
.card-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; padding-bottom: 1rem; border-bottom: 1px solid #e5e7eb; }
.card-header h3 { font-size: 1.1rem; color: #1a1a2e; }
.card-body { flex: 1; }
.card-body p { margin: 0.35rem 0; color: #4b5563; font-size: 0.95rem; }
.rental-info { background: #f3f4f6; padding: 0.75rem; border-radius: 8px; margin-top: 0.75rem; }
.downtime-info { background: #fef3c7; padding: 0.75rem; border-radius: 8px; margin-top: 0.75rem; }
.card-footer { display: flex; gap: 0.5rem; flex-wrap: wrap; margin-top: 1rem; padding-top: 1rem; border-top: 1px solid #e5e7eb; }
.btn-sm { padding: 0.5rem 1rem; font-size: 0.875rem; }
.btn:disabled { opacity: 0.5; cursor: not-allowed; }

.view-toggle {
  display: flex;
  gap: 0.5rem;
  margin-left: auto;
  flex-wrap: wrap;
}

.view-toggle__btn {
  padding: 0.5rem 0.9rem;
  border-radius: 999px;
  border: 1px solid #d1d5db;
  background: #fff;
  font-size: 0.85rem;
  cursor: pointer;
}

.view-toggle__btn--active {
  background: #1a1a2e;
  border-color: #1a1a2e;
  color: #fff;
}
@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
</style>
