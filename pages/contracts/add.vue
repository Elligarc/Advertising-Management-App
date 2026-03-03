<template>
  <div class="add-contract-page">
    <div class="page-header">
      <h1 class="page-title">Новый договор</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/contracts" class="tab">Список</NuxtLink>
      <span class="tab tab--active">Новый договор</span>
    </nav>

    <div class="form-card card">
      <form @submit.prevent="submit">
        <div class="form-grid">
          <div class="form-column">
            <div class="form-group">
              <label>Номер договора *</label>
              <input v-model="form.number" type="text" required placeholder="Д-001/26">
            </div>

            <div class="form-group">
              <label>Клиент *</label>
              <select v-model="form.clientId" required>
                <option value="">Выберите клиента</option>
                <option v-for="c in clients" :key="c.id" :value="c.id">
                  {{ c.name }}
                </option>
              </select>
            </div>

            <div class="form-group">
              <label>Период размещения *</label>
              <div class="inline-fields">
                <input v-model="form.startDate" type="date" required>
                <span>—</span>
                <input v-model="form.endDate" type="date" required>
              </div>
            </div>
          </div>

          <div class="form-column">
            <div class="form-group">
              <label>Комментарий</label>
              <textarea v-model="form.comment" rows="4" placeholder="Особые условия, оплата, макеты и т.п."></textarea>
            </div>
          </div>
        </div>

        <div class="surfaces-block">
          <h2>Поверхности в договоре</h2>
          <p class="hint">Можно выбрать несколько поверхностей. Кнопка «Добавить поверхность в договор» на списке поверхностей будет приводить сюда с уже отмеченной поверхностью.</p>

          <div class="surfaces-list">
            <label
              v-for="s in surfaces"
              :key="s.id"
              class="surface-item"
            >
              <input
                v-model="form.surfaceIds"
                type="checkbox"
                :value="s.id"
              >
              <div class="surface-item__info">
                <div class="surface-item__title">
                  {{ s.name }}
                  <span class="surface-item__type">{{ s.type }}</span>
                </div>
                <div class="surface-item__meta">
                  <span>{{ s.address }}</span>
                  <span>{{ s.price }} ₽/день</span>
                </div>
              </div>
            </label>
          </div>
        </div>

        <div class="form-actions">
          <NuxtLink to="/contracts" class="btn btn-secondary">Отмена</NuxtLink>
          <button type="submit" class="btn btn-primary">Сохранить договор</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const route = useRoute()

const clients = ref([
  { id: 1, name: 'ООО "Ромашка"' },
  { id: 2, name: 'ООО "ТехноПлюс"' },
  { id: 3, name: 'ИП Сидоров' }
])

const surfaces = ref([
  { id: 1, name: 'Билборд Тверская 15', address: 'г. Москва, ул. Тверская, д.15', type: 'Билборд 3x6', price: 5000 },
  { id: 2, name: 'Ситилайт Арбат 10', address: 'г. Москва, ул. Арбат, д.10', type: 'Ситилайт 1.2x1.8', price: 3000 },
  { id: 3, name: 'Видеоэкран Садовая 5', address: 'г. Москва, ул. Садовая, д.5', type: 'Видеоэкран', price: 8000 },
  { id: 4, name: 'Билборд Ленинский 20', address: 'г. Москва, Ленинский пр-т, д.20', type: 'Билборд 3x6', price: 5500 }
])

const form = ref({
  number: '',
  clientId: '',
  startDate: '',
  endDate: '',
  comment: '',
  surfaceIds: []
})

onMounted(() => {
  const surfaceFromQuery = route.query.surface
  if (surfaceFromQuery) {
    const id = Number(surfaceFromQuery)
    if (!Number.isNaN(id) && !form.value.surfaceIds.includes(id)) {
      form.value.surfaceIds.push(id)
    }
  }
})

function submit() {
  // В реальном приложении здесь будет вызов API.
  navigateTo('/contracts')
}
</script>

<style scoped>
.add-contract-page {
  animation: fadeIn 0.3s ease;
}

.page-header {
  margin-bottom: 1rem;
}

.page-title {
  font-size: 1.75rem;
  color: #1a1a2e;
  font-weight: 600;
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

.tab--active {
  color: #1e3c72;
  border-bottom-color: #1e3c72;
}

.form-card {
  padding: 1.75rem 1.75rem 1.5rem;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(260px, 1fr));
  gap: 1.5rem;
  margin-bottom: 1.5rem;
}

.form-group {
  margin-bottom: 1.25rem;
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
  font-size: 0.95rem;
}

.inline-fields {
  display: grid;
  grid-template-columns: 1fr auto 1fr;
  gap: 0.5rem;
  align-items: center;
}

.surfaces-block h2 {
  font-size: 1.1rem;
  margin-bottom: 0.25rem;
}

.hint {
  font-size: 0.85rem;
  color: #6b7280;
  margin-bottom: 0.75rem;
}

.surfaces-list {
  max-height: 260px;
  overflow: auto;
  padding: 0.5rem;
  border-radius: 8px;
  border: 1px solid #e5e7eb;
  background: #f9fafb;
}

.surface-item {
  display: flex;
  gap: 0.75rem;
  padding: 0.5rem 0.4rem;
  align-items: flex-start;
}

.surface-item + .surface-item {
  border-top: 1px solid #e5e7eb;
}

.surface-item__info {
  flex: 1;
}

.surface-item__title {
  font-size: 0.95rem;
  font-weight: 500;
  color: #111827;
}

.surface-item__type {
  margin-left: 0.5rem;
  font-size: 0.8rem;
  color: #6b7280;
}

.surface-item__meta {
  margin-top: 0.25rem;
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  font-size: 0.8rem;
  color: #6b7280;
}

.form-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 1.5rem;
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

