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
        placeholder="Поиск по адресу..."
        class="search-input"
      >
      <select v-model="cityFilter" class="select-filter">
        <option value="all">Все города</option>
        <option v-for="city in cities" :key="city.id" :value="city.id">
          {{ city.name }}
        </option>
      </select>
      <select v-model="districtFilter" class="select-filter">
        <option value="all">Все районы</option>
        <option v-for="district in filteredDistricts" :key="district.id" :value="district.id">
          {{ district.name }}
        </option>
      </select>
      <select v-model="formatFilter" class="select-filter">
        <option value="all">Все форматы</option>
        <option v-for="format in formats" :key="format.id" :value="format.id">
          {{ format.name }}
        </option>
      </select>
      <select v-model="typeFilter" class="select-filter">
        <option value="all">Все типы</option>
        <option value="Billboard">Билборд</option>
        <option value="Poster">Постер</option>
        <option value="CityLight">Ситилайт</option>
        <option value="Videoboard">Видеоэкран</option>
      </select>
      <select v-model="statusFilter" class="select-filter">
        <option value="all">Все статусы</option>
        <option value="Created">Активна</option>
        <option value="UnderRepair">На ремонте</option>
        <option value="Decommissioned">Выведена</option>
      </select>
      <select v-model="priceTypeFilter" class="select-filter">
        <option value="all">Любой тип цены</option>
        <option value="PerMonth">За месяц</option>
        <option value="PerShow">За показ</option>
      </select>
      <button class="btn btn-secondary" @click="resetFilters">Сбросить</button>

      <div class="view-toggle">
        <button type="button" class="view-toggle__btn" :class="{ 'view-toggle__btn--active': viewMode === 'normal' }" @click="viewMode = 'normal'">
          Крупные карточки
        </button>
        <button type="button" class="view-toggle__btn" :class="{ 'view-toggle__btn--active': viewMode === 'compact' }" @click="viewMode = 'compact'">
          Компактно
        </button>
      </div>
    </div>

    <div v-if="pending" class="state-msg">Загрузка...</div>
    <div v-else-if="error" class="state-msg error-msg">Ошибка загрузки поверхностей</div>

    <div v-else class="grid">
      <div
        v-for="surface in filteredSurfaces"
        :key="surface.id"
        :class="['surface-card', 'card', viewMode === 'compact' ? 'surface-card--compact' : '']"
      >
        <div class="card-header">
          <h3>{{ surface.construction?.address }}</h3>
          <span :class="['status', statusClass(surface.currentStatus)]">{{ statusText(surface.currentStatus) }}</span>
        </div>
        <div class="card-body">
          <p class="type">{{ surface.construction?.format?.constructionType }} · Сторона {{ surface.side }} · {{ surface.surfaceType === 'Digital' ? 'Цифровая' : 'Статичная' }}</p>
          <p class="price">{{ surface.currentPrice }} ₽ / {{ surface.currentPriceType === 'PerMonth' ? 'мес' : 'показ' }}</p>
          <p v-if="surface.loopDuration" class="meta">Петля: {{ surface.loopDuration }} сек · Слот: {{ surface.slotDuration }} сек · Макс. слотов: {{ surface.maxSlots }}</p>
        </div>
        <div class="card-footer">
          <NuxtLink :to="`/surfaces/${surface.id}`" class="btn btn-primary btn-sm">Подробнее</NuxtLink>
          <NuxtLink :to="`/surfaces/${surface.id}/edit`" class="btn btn-secondary btn-sm">Редактировать</NuxtLink>
          <button class="btn btn-secondary btn-sm" type="button" @click="addToContract(surface.id)">
            Добавить в договор
          </button>
          <button class="btn btn-success btn-sm" :disabled="surface.currentStatus !== 'Created'" @click="rentSurface(surface.id)">
            Сдать в аренду
          </button>
        </div>
      </div>

      <div v-if="filteredSurfaces.length === 0" class="state-msg">Поверхности не найдены</div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useCities } from '~/composable/useCities'
import { useDistricts } from '~/composable/useDistricts'
import { useFormats } from '~/composable/useFormats'
import { useSurfaces } from '~/composable/useSurfaces'

const searchQuery = ref('')
const typeFilter = ref('all')
const statusFilter = ref('all')
const priceTypeFilter = ref('all')
const cityFilter = ref('all')
const districtFilter = ref('all')
const formatFilter = ref('all')
const viewMode = ref('normal')

const { cities } = useCities()
const { districts, getDistrictsByCity } = useDistricts()
const { formats } = useFormats()

const { surfaces, loading: pending, error, fetchSurfaces } = useSurfaces()

// Инициализация загрузки поверхностей
await fetchSurfaces()

const filteredDistricts = computed(() => {
  if (cityFilter.value === 'all') {
    return districts.value
  }
  return getDistrictsByCity(cityFilter.value)
})

const filteredSurfaces = computed(() => {
  return surfaces.value.filter(s => {
    const q = searchQuery.value.toLowerCase()
    const address = s.construction?.address?.toLowerCase() ?? ''
    const matchSearch = !q || address.includes(q)
    const matchCity = cityFilter.value === 'all' || s.construction?.city?.id === Number(cityFilter.value)
    const matchDistrict = districtFilter.value === 'all' || s.construction?.district?.id === Number(districtFilter.value)
    const matchFormat = formatFilter.value === 'all' || s.construction?.format?.id === Number(formatFilter.value)
    const matchType = typeFilter.value === 'all' || s.construction?.format?.constructionType === typeFilter.value
    const matchStatus = statusFilter.value === 'all' || s.currentStatus === statusFilter.value
    const matchPrice = priceTypeFilter.value === 'all' || s.currentPriceType === priceTypeFilter.value
    return matchSearch && matchCity && matchDistrict && matchFormat && matchType && matchStatus && matchPrice
  })
})

function statusClass(status) {
  return { Created: 'status-free', UnderRepair: 'status-repair', Decommissioned: 'status-busy' }[status] ?? ''
}

function statusText(status) {
  return { Created: 'Активна', UnderRepair: 'На ремонте', Decommissioned: 'Выведена' }[status] ?? status
}

function resetFilters() {
  searchQuery.value = ''
  cityFilter.value = 'all'
  districtFilter.value = 'all'
  formatFilter.value = 'all'
  typeFilter.value = 'all'
  statusFilter.value = 'all'
  priceTypeFilter.value = 'all'
}

function rentSurface(id) {
  navigateTo(`/surfaces/rent/${id}`)
}

function addToContract(id) {
  navigateTo(`/contracts/add?surface=${id}`)
}
</script>

<style scoped>
.surfaces-page { animation: fadeIn 0.3s ease; }
.page-header { margin-bottom: 1rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; }

.tabs { display: flex; gap: 0.25rem; margin-bottom: 1.5rem; border-bottom: 2px solid #e5e7eb; }
.tab { padding: 0.75rem 1.25rem; color: #6b7280; text-decoration: none; font-weight: 500; border-bottom: 2px solid transparent; margin-bottom: -2px; transition: color 0.2s, border-color 0.2s; }
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }

.surface-card { display: flex; flex-direction: column; }
.surface-card--compact { padding: 0.75rem 1rem; }
.surface-card--compact .card-header { margin-bottom: 0.5rem; padding-bottom: 0.5rem; }
.surface-card--compact .card-body p { font-size: 0.85rem; margin: 0.25rem 0; }
.surface-card--compact .card-footer { margin-top: 0.5rem; padding-top: 0.5rem; }
.card-header { display: flex; justify-content: space-between; align-items: center; margin-bottom: 1rem; padding-bottom: 1rem; border-bottom: 1px solid #e5e7eb; }
.card-header h3 { font-size: 1.1rem; color: #1a1a2e; }
.card-body { flex: 1; }
.card-body p { margin: 0.35rem 0; color: #4b5563; font-size: 0.95rem; }
.card-footer { display: flex; gap: 0.5rem; flex-wrap: wrap; margin-top: 1rem; padding-top: 1rem; border-top: 1px solid #e5e7eb; }
.btn-sm { padding: 0.5rem 1rem; font-size: 0.875rem; }
.btn:disabled { opacity: 0.5; cursor: not-allowed; }

.view-toggle { display: flex; gap: 0.5rem; margin-left: auto; flex-wrap: wrap; }
.view-toggle__btn { padding: 0.5rem 0.9rem; border-radius: 999px; border: 1px solid #d1d5db; background: #fff; font-size: 0.85rem; cursor: pointer; }
.view-toggle__btn--active { background: #1a1a2e; border-color: #1a1a2e; color: #fff; }

.state-msg { padding: 2rem; text-align: center; color: #6b7280; }
.error-msg { color: #e53e3e; background: #fff5f5; border-radius: 8px; }

@keyframes fadeIn { from { opacity: 0; } to { opacity: 1; } }
</style>