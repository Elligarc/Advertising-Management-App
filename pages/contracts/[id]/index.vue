<template>
  <div class="contract-detail">
    <div class="page-header">
      <NuxtLink to="/contracts" class="back-link">К списку договоров</NuxtLink>
      <h1 class="page-title">
        {{ contract ? `Договор ${contract.number}` : 'Договор' }}
      </h1>
    </div>

    <nav class="tabs">
      <button
        type="button"
        class="tab"
        :class="{ 'tab--active': activeTab === 'info' }"
        @click="activeTab = 'info'"
      >
        Общая информация
      </button>
      <button
        type="button"
        class="tab"
        :class="{ 'tab--active': activeTab === 'surfaces' }"
        @click="activeTab = 'surfaces'"
      >
        Поверхности
      </button>
    </nav>

    <div v-if="!contract" class="empty">
      Договор не найден.
    </div>

    <div v-else>
      <!-- Вкладка: общая информация -->
      <div v-if="activeTab === 'info'" class="card info-card">
        <div class="info-row">
          <span class="label">Номер</span>
          <span class="value">{{ contract.number }}</span>
        </div>
        <div class="info-row">
          <span class="label">Клиент</span>
          <span class="value">{{ contract.client }}</span>
        </div>
        <div class="info-row">
          <span class="label">Период</span>
          <span class="value">{{ contract.startDate }} — {{ contract.endDate }}</span>
        </div>
        <div class="info-row">
          <span class="label">Поверхностей</span>
          <span class="value">{{ contract.surfaces.length }}</span>
        </div>
        <div class="info-row">
          <span class="label">Сумма</span>
          <span class="value">{{ contract.total }} ₽</span>
        </div>
        <div class="info-row">
          <span class="label">Статус</span>
          <span class="value">
            <span class="status-badge" :class="`status-badge--${contract.status}`">
              {{ statusText(contract.status) }}
            </span>
          </span>
        </div>
      </div>

      <!-- Вкладка: поверхности -->
      <div v-else class="surfaces-tab">
        <div class="surfaces-toolbar">
          <div class="filters-inline">
            <select v-model="statusFilter" class="select-filter">
              <option value="all">Все статусы</option>
              <option value="free">Свободны</option>
              <option value="busy">Заняты</option>
              <option value="repair">В простое</option>
            </select>
          </div>
        </div>

        <div class="card surfaces-list">
          <table>
            <thead>
              <tr>
                <th>Поверхность</th>
                <th>Адрес</th>
                <th>Тип</th>
                <th>Статус</th>
                <th>Цена, ₽/день</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="s in filteredSurfaces" :key="s.id">
                <td>{{ s.name }}</td>
                <td>{{ s.address }}</td>
                <td>{{ s.type }}</td>
                <td>
                  <span class="status" :class="s.statusClass">
                    {{ s.statusText }}
                  </span>
                </td>
                <td>{{ s.price }}</td>
              </tr>
              <tr v-if="filteredSurfaces.length === 0">
                <td colspan="5" class="empty">Нет поверхностей по заданным условиям.</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const route = useRoute()
const id = computed(() => Number(route.params.id))

const activeTab = ref('info')
const statusFilter = ref('all')

// Тестовые данные договоров с привязанными поверхностями
const contracts = ref([
  {
    id: 1,
    number: 'Д-001/26',
    client: 'ООО "Ромашка"',
    startDate: '01.03.2026',
    endDate: '31.03.2026',
    total: 320000,
    status: 'active',
    surfaces: [
      {
        id: 1,
        name: 'Билборд Тверская 15',
        address: 'г. Москва, ул. Тверская, д.15',
        type: 'Билборд 3x6',
        price: 5000,
        status: 'busy',
        statusClass: 'status-busy',
        statusText: 'Занята',
        currentRental: {
          client: 'ООО "Ромашка"'
        }
      },
      {
        id: 3,
        name: 'Видеоэкран Садовая 5',
        address: 'г. Москва, ул. Садовая, д.5',
        type: 'Видеоэкран',
        price: 8000,
        status: 'free',
        statusClass: 'status-free',
        statusText: 'Свободна'
      }
    ]
  },
  {
    id: 2,
    number: 'Д-002/26',
    client: 'ООО "ТехноПлюс"',
    startDate: '01.04.2026',
    endDate: '30.06.2026',
    total: 580000,
    status: 'planned',
    surfaces: [
      {
        id: 2,
        name: 'Ситилайт Арбат 10',
        address: 'г. Москва, ул. Арбат, д.10',
        type: 'Ситилайт 1.2x1.8',
        price: 3000,
        status: 'busy',
        statusClass: 'status-busy',
        statusText: 'Занята',
        currentRental: {
          client: 'ООО "ТехноПлюс"'
        }
      }
    ]
  }
])

const contract = computed(() =>
  contracts.value.find(c => c.id === id.value)
)

const filteredSurfaces = computed(() => {
  if (!contract.value) return []
  return contract.value.surfaces.filter(s => {
    if (statusFilter.value === 'all') return true
    return s.status === statusFilter.value
  })
})

function statusText(status) {
  if (status === 'active') return 'Активен'
  if (status === 'planned') return 'Запланирован'
  return 'Завершён'
}
</script>

<style scoped>
.contract-detail {
  animation: fadeIn 0.3s ease;
}

.page-header {
  margin-bottom: 1rem;
}

.back-link {
  color: #1e3c72;
  text-decoration: none;
  font-size: 0.9rem;
  display: inline-block;
  margin-bottom: 0.25rem;
}

.back-link:hover {
  text-decoration: underline;
}

.page-title {
  font-size: 1.75rem;
  color: #1a1a2e;
  font-weight: 600;
}

.tabs {
  display: flex;
  gap: 0.25rem;
  margin: 1rem 0 1.5rem;
  border-bottom: 2px solid #e5e7eb;
}

.tab {
  padding: 0.75rem 1.25rem;
  color: #6b7280;
  text-decoration: none;
  font-weight: 500;
  border-bottom: 2px solid transparent;
  margin-bottom: -2px;
  font-size: 0.95rem;
  background: transparent;
  border-radius: 0;
  cursor: pointer;
}

.tab--active {
  color: #1e3c72;
  border-bottom-color: #1e3c72;
}

.info-card {
  padding: 1.5rem;
}

.info-row {
  display: flex;
  gap: 1rem;
  padding: 0.5rem 0;
  border-bottom: 1px solid #e5e7eb;
}

.info-row .label {
  min-width: 140px;
  color: #6b7280;
}

.info-row .value {
  color: #111827;
  font-weight: 500;
}

.status-badge {
  display: inline-flex;
  align-items: center;
  padding: 0.15rem 0.55rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
}

.status-badge--active {
  background: #d1fae5;
  color: #065f46;
}

.status-badge--planned {
  background: #e0f2fe;
  color: #0369a1;
}

.status-badge--finished {
  background: #e5e7eb;
  color: #4b5563;
}

.surfaces-toolbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
  gap: 1rem;
  flex-wrap: wrap;
}

.view-toggle {
  display: flex;
  gap: 0.5rem;
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

.surfaces-list table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.9rem;
}

.surfaces-list th,
.surfaces-list td {
  padding: 0.55rem 0.75rem;
  text-align: left;
}

.surfaces-list thead tr {
  border-bottom: 1px solid #e5e7eb;
}

.surfaces-list tbody tr + tr {
  border-top: 1px solid #f3f4f6;
}

.surfaces-list th {
  font-weight: 600;
  color: #6b7280;
  font-size: 0.8rem;
  text-transform: uppercase;
}

.kanban {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
}

.kanban-column {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.kanban-column__header {
  font-size: 0.9rem;
  font-weight: 600;
  color: #374151;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.kanban-column__header .count {
  font-size: 0.75rem;
  background: #e5e7eb;
  border-radius: 999px;
  padding: 0.1rem 0.45rem;
}

.kanban-column__body {
  padding: 0.5rem;
  border-radius: 8px;
  background: #f9fafb;
  border: 1px solid #e5e7eb;
  min-height: 80px;
}

.kanban-card {
  padding: 0.5rem 0.6rem;
  border-radius: 6px;
  background: #fff;
  box-shadow: 0 1px 2px rgba(0, 0, 0, 0.04);
  margin-bottom: 0.5rem;
}

.kanban-card .title {
  font-size: 0.9rem;
  font-weight: 600;
  margin-bottom: 0.25rem;
}

.kanban-card .meta {
  font-size: 0.8rem;
  color: #6b7280;
}

.empty {
  color: #6b7280;
}

.empty-column {
  font-size: 0.8rem;
  color: #9ca3af;
  text-align: center;
  margin: 0.25rem 0;
}

@keyframes fadeIn {
  from {
    opacity: 0;
  }
  to {
    opacity: 1;
  }
}
</style>

