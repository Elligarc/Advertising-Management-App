<template>
  <div class="edit-page">
    <div class="page-header">
      <NuxtLink to="/downtime" class="back-link">К списку простоев</NuxtLink>
      <h1 class="page-title">Редактирование простоя</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/downtime" class="tab">Список</NuxtLink>
      <NuxtLink to="/downtime/add" class="tab">Добавить простой</NuxtLink>
      <span class="tab tab--active">Редактирование</span>
    </nav>

    <div v-if="form" class="form-card card">
      <form @submit.prevent="submit">
        <div class="form-group">
          <label>Поверхность *</label>
          <select v-model="form.surfaceId" required>
            <option value="">Выберите поверхность</option>
            <option v-for="s in surfaces" :key="s.id" :value="s.id">
              {{ s.name }}
            </option>
          </select>
        </div>

        <div class="form-group">
          <label>Причина простоя *</label>
          <input
            v-model="form.reason"
            type="text"
            required
            placeholder="Ремонт, замена подсветки, ТО..."
          >
        </div>

        <div class="form-group">
          <label>Дата начала *</label>
          <input v-model="form.startDate" type="date" required>
        </div>

        <div class="form-group">
          <label>Дата окончания</label>
          <input v-model="form.endDate" type="date">
        </div>

        <div class="form-group">
          <label>Комментарий</label>
          <textarea v-model="form.comment" rows="3"></textarea>
        </div>

        <div class="form-actions">
          <NuxtLink to="/downtime" class="btn btn-secondary">Отмена</NuxtLink>
          <button type="submit" class="btn btn-primary">Сохранить</button>
        </div>
      </form>
    </div>

    <p v-else class="empty">
      Простой не найден.
    </p>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'

const route = useRoute()
const id = computed(() => Number(route.params.id))

// Тестовые поверхности (та же логика, что и в других разделах)
const surfaces = ref([
  { id: 1, name: 'Билборд Тверская 15' },
  { id: 2, name: 'Ситилайт Арбат 10' },
  { id: 3, name: 'Видеоэкран Садовая 5' },
  { id: 4, name: 'Билборд Ленинский 20' },
  { id: 5, name: 'Баннер МКАД 45 км' }
])

// Тестовые простои — синхронизированы с /downtime
const downtimes = ref([
  {
    id: 1,
    surfaceId: 2,
    surface: 'Ситилайт Арбат 10',
    reason: 'Замена подсветки',
    startDate: '2026-03-01',
    endDate: '2026-03-10',
    comment: '',
    isActive: false
  },
  {
    id: 2,
    surfaceId: 1,
    surface: 'Билборд Тверская 15',
    reason: 'Ремонт конструкции',
    startDate: '2026-03-15',
    endDate: '',
    comment: 'Ожидается поставка деталей',
    isActive: true
  }
])

const form = ref(null)

watch(
  id,
  newId => {
    const item = downtimes.value.find(d => d.id === newId)
    if (!item) {
      form.value = null
      return
    }
    form.value = {
      surfaceId: item.surfaceId || surfaces.value.find(s => s.name === item.surface)?.id || '',
      reason: item.reason || '',
      startDate: item.startDate || '',
      endDate: item.endDate || '',
      comment: item.comment || ''
    }
  },
  { immediate: true }
)

function submit() {
  // В реальном приложении здесь будет вызов API.
  navigateTo('/downtime')
}
</script>

<style scoped>
.edit-page {
  max-width: 640px;
}

.page-header {
  margin-bottom: 1rem;
}

.page-title {
  font-size: 1.75rem;
  color: #1a1a2e;
  font-weight: 600;
}

.back-link {
  color: #1e3c72;
  text-decoration: none;
  font-size: 0.95rem;
  margin-bottom: 0.25rem;
  display: inline-block;
}

.back-link:hover {
  text-decoration: underline;
}

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
  font-size: 0.95rem;
}

.tab:hover {
  color: #1e3c72;
}

.tab--active {
  color: #1e3c72;
  border-bottom-color: #1e3c72;
}

.form-card {
  padding: 2rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #1a1a2e;
  font-weight: 500;
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #d1d5db;
  border-radius: 8px;
  font-size: 1rem;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #1a1a2e;
}

.form-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;
}

.empty {
  color: #6b7280;
}
</style>

