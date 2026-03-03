<template>
  <div class="add-page">
    <div class="page-header">
      <NuxtLink to="/downtime" class="back-link">← К списку простоев</NuxtLink>
      <h1 class="page-title">Отметить простой поверхности</h1>
    </div>

    <div class="form-card card">
      <form @submit.prevent="submit">
        <div class="form-group">
          <label>Поверхность *</label>
          <select v-model="form.surfaceId" required>
            <option value="">Выберите поверхность</option>
            <option v-for="s in surfaces" :key="s.id" :value="s.id">{{ s.name }}</option>
          </select>
        </div>
        <div class="form-group">
          <label>Причина простоя *</label>
          <input v-model="form.reason" type="text" required placeholder="Ремонт, замена подсветки, ТО...">
        </div>
        <div class="form-group">
          <label>Дата начала *</label>
          <input v-model="form.startDate" type="date" required>
        </div>
        <div class="form-group">
          <label>Ожидаемая дата окончания</label>
          <input v-model="form.endDate" type="date">
        </div>
        <div class="form-group">
          <label>Комментарий</label>
          <textarea v-model="form.comment" rows="3"></textarea>
        </div>
        <div class="form-actions">
          <NuxtLink to="/downtime" class="btn">Отмена</NuxtLink>
          <button type="submit" class="btn btn-warning">Сохранить простой</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const form = ref({
  surfaceId: '',
  reason: '',
  startDate: new Date().toISOString().slice(0, 10),
  endDate: '',
  comment: ''
})

const surfaces = ref([
  { id: 1, name: 'Билборд Тверская 15' },
  { id: 2, name: 'Ситилайт Арбат 10' },
  { id: 3, name: 'Видеоэкран Садовая 5' },
  { id: 4, name: 'Билборд Ленинский 20' },
  { id: 5, name: 'Баннер МКАД 45 км' }
])

const route = useRoute()
if (route.query.surface) {
  form.value.surfaceId = Number(route.query.surface)
}

function submit() {
  navigateTo('/downtime')
}
</script>

<style scoped>
.add-page {
  max-width: 600px;
  animation: fadeIn 0.5s;
}

.back-link {
  color: #2a5298;
  text-decoration: none;
  margin-bottom: 1rem;
  display: inline-block;
}

.back-link:hover {
  text-decoration: underline;
}

.page-title {
  font-size: 1.75rem;
  color: #2c3e50;
  margin-bottom: 1.5rem;
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
  color: #2c3e50;
  font-weight: 500;
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #2a5298;
}

.form-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;
}

@keyframes fadeIn {
  from { opacity: 0; transform: translateY(20px); }
  to { opacity: 1; transform: translateY(0); }
}
</style>
