<template>
  <div class="downtime-page">
    <div class="page-header">
      <h1 class="page-title">Простои поверхностей</h1>
      <NuxtLink to="/downtime/add" class="btn btn-warning">Отметить простой</NuxtLink>
    </div>

    <div class="filters">
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Поиск по поверхности или причине..."
        class="search-input"
      >
      <select v-model="statusFilter" class="select-filter">
        <option value="all">Все</option>
        <option value="active">Активные</option>
        <option value="ended">Завершённые</option>
      </select>
    </div>

    <div class="downtime-grid">
      <div v-for="item in filteredDowntimes" :key="item.id" class="downtime-card card">
        <div class="card-header">
          <h3>{{ item.surface }}</h3>
          <span class="badge" :class="item.isActive ? 'badge-active' : 'badge-ended'">
            {{ item.isActive ? 'Активен' : 'Завершён' }}
          </span>
        </div>
        <p class="reason">{{ item.reason }}</p>
        <p class="dates">{{ item.startDate }} — {{ item.endDate || 'по настоящее время' }}</p>
        <div v-if="item.comment" class="comment">{{ item.comment }}</div>
        <div class="card-footer">
          <NuxtLink :to="`/downtime/${item.id}/edit`" class="btn btn-primary btn-sm">Изменить</NuxtLink>
          <button v-if="item.isActive" class="btn btn-success btn-sm" @click="endDowntime(item.id)">Завершить</button>
        </div>
      </div>
    </div>

    <div v-if="filteredDowntimes.length === 0" class="empty">
      <p>Нет записей о простоях. <NuxtLink to="/downtime/add">Добавить простой</NuxtLink></p>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'

const searchQuery = ref('')
const statusFilter = ref('all')

const downtimes = ref([
  {
    id: 1,
    surface: 'Ситилайт Арбат 10',
    reason: 'Замена подсветки',
    startDate: '01.03.2026',
    endDate: '10.03.2026',
    comment: '',
    isActive: false
  },
  {
    id: 2,
    surface: 'Билборд Тверская 15',
    reason: 'Ремонт конструкции',
    startDate: '15.03.2026',
    endDate: null,
    comment: 'Ожидается поставка деталей',
    isActive: true
  }
])

const filteredDowntimes = computed(() => {
  let list = downtimes.value
  const q = searchQuery.value.toLowerCase()
  if (q) {
    list = list.filter(d =>
      d.surface.toLowerCase().includes(q) || d.reason.toLowerCase().includes(q)
    )
  }
  if (statusFilter.value === 'active') list = list.filter(d => d.isActive)
  if (statusFilter.value === 'ended') list = list.filter(d => !d.isActive)
  return list
})

function endDowntime() {
  // В реальном приложении — вызов API
}
</script>

<style scoped>
.downtime-page {
  animation: fadeIn 0.5s;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
  margin-bottom: 2rem;
}

.page-title {
  font-size: 2rem;
  color: #2c3e50;
}

.downtime-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
  gap: 1.5rem;
}

.downtime-card .card-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.75rem;
  padding-bottom: 0.75rem;
  border-bottom: 1px solid #eee;
}

.downtime-card h3 {
  font-size: 1.1rem;
  color: #2c3e50;
}

.badge {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.8rem;
  font-weight: 600;
}

.badge-active {
  background: #fff3cd;
  color: #856404;
}

.badge-ended {
  background: #e9ecef;
  color: #6c757d;
}

.reason {
  font-weight: 500;
  color: #856404;
  margin: 0.5rem 0;
}

.dates {
  color: #7f8c8d;
  font-size: 0.95rem;
  margin: 0.5rem 0;
}

.comment {
  margin-top: 0.75rem;
  padding: 0.5rem;
  background: #f8f9fa;
  border-radius: 6px;
  font-size: 0.9rem;
  color: #6c757d;
}

.downtime-card .card-footer {
  display: flex;
  gap: 0.5rem;
  margin-top: 1rem;
  padding-top: 1rem;
  border-top: 1px solid #eee;
}

.btn-sm {
  padding: 0.5rem 1rem;
  font-size: 0.875rem;
}

.empty {
  text-align: center;
  padding: 3rem;
  color: #7f8c8d;
}

.empty a {
  color: #2a5298;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
