<template>
  <div class="contracts-page">
    <div class="page-header">
      <h1 class="page-title">Договоры</h1>
      <NuxtLink to="/contracts/add" class="btn btn-primary">Новый договор</NuxtLink>
    </div>

    <div class="filters">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Поиск по номеру или клиенту..."
        class="search-input"
      >
      <select v-model="statusFilter" class="select-filter">
        <option value="all">Все статусы</option>
        <option value="active">Активные</option>
        <option value="planned">Запланированные</option>
        <option value="finished">Завершённые</option>
      </select>
    </div>

    <div class="contracts-table card">
      <table>
        <thead>
          <tr>
            <th>№</th>
            <th>Клиент</th>
            <th>Период</th>
            <th>Поверхностей</th>
            <th>Сумма</th>
            <th>Статус</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="contract in filteredContracts" :key="contract.id">
            <td>{{ contract.number }}</td>
            <td>{{ contract.client }}</td>
            <td>{{ contract.startDate }} — {{ contract.endDate }}</td>
            <td>{{ contract.surfacesCount }}</td>
            <td>{{ contract.total }} ₽</td>
            <td>
              <span class="status-badge" :class="`status-badge--${contract.status}`">
                {{ statusText(contract.status) }}
              </span>
            </td>
            <td class="actions">
              <NuxtLink :to="`/contracts/${contract.id}`" class="btn btn-secondary btn-sm">Открыть</NuxtLink>
            </td>
          </tr>
          <tr v-if="filteredContracts.length === 0">
            <td colspan="7" class="empty">Договоров пока нет.</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const searchQuery = ref('')
const statusFilter = ref('all')

const contracts = ref([
  {
    id: 1,
    number: 'Д-001/26',
    client: 'ООО "Ромашка"',
    startDate: '01.03.2026',
    endDate: '31.03.2026',
    surfacesCount: 3,
    total: 320000,
    status: 'active'
  },
  {
    id: 2,
    number: 'Д-002/26',
    client: 'ООО "ТехноПлюс"',
    startDate: '01.04.2026',
    endDate: '30.06.2026',
    surfacesCount: 2,
    total: 580000,
    status: 'planned'
  },
  {
    id: 3,
    number: 'Д-099/25',
    client: 'ИП Сидоров',
    startDate: '01.01.2025',
    endDate: '31.12.2025',
    surfacesCount: 1,
    total: 240000,
    status: 'finished'
  }
])

const filteredContracts = computed(() => {
  const q = searchQuery.value.toLowerCase()
  return contracts.value.filter(c => {
    const matchSearch =
      !q ||
      c.number.toLowerCase().includes(q) ||
      c.client.toLowerCase().includes(q)
    const matchStatus =
      statusFilter.value === 'all' || c.status === statusFilter.value
    return matchSearch && matchStatus
  })
})

function statusText(status) {
  if (status === 'active') return 'Активен'
  if (status === 'planned') return 'Запланирован'
  return 'Завершён'
}
</script>

<style scoped>
.contracts-page {
  animation: fadeIn 0.3s ease;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.page-title {
  font-size: 1.75rem;
  color: #1a1a2e;
  font-weight: 600;
}

.contracts-table table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.95rem;
}

th,
td {
  padding: 0.6rem 0.75rem;
  text-align: left;
}

thead tr {
  border-bottom: 1px solid #e5e7eb;
}

tbody tr + tr {
  border-top: 1px solid #f3f4f6;
}

th {
  font-weight: 600;
  color: #6b7280;
  font-size: 0.85rem;
  text-transform: uppercase;
}

td {
  color: #1f2933;
}

.actions {
  text-align: right;
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

.empty {
  text-align: center;
  color: #6b7280;
  padding: 1.25rem 0;
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

