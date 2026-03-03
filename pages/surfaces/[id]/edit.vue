<template>
  <div class="edit-page">
    <div class="page-header">
      <NuxtLink to="/surfaces" class="back-link">К списку поверхностей</NuxtLink>
      <h1 class="page-title">{{ isNew ? 'Новая поверхность' : 'Редактирование поверхности' }}</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/surfaces" class="tab">Список</NuxtLink>
      <NuxtLink to="/surfaces/add" class="tab">Добавить поверхность</NuxtLink>
      <NuxtLink v-if="!isNew && id" :to="`/surfaces/${id}`" class="tab">Просмотр</NuxtLink>
    </nav>

    <div class="form-card card">
      <form @submit.prevent="submit">
        <div class="form-group">
          <label>Название *</label>
          <input v-model="form.name" type="text" required placeholder="Например: Билборд Тверская 15">
        </div>
        <div class="form-group">
          <label>Адрес *</label>
          <input v-model="form.address" type="text" required placeholder="г. Москва, ул. ...">
        </div>
        <div class="form-group">
          <label>Тип *</label>
          <select v-model="form.type" required>
            <option value="Билборд 3x6">Билборд 3x6</option>
            <option value="Ситилайт">Ситилайт</option>
            <option value="Видеоэкран">Видеоэкран</option>
            <option value="Баннер">Баннер</option>
          </select>
        </div>
        <div class="form-group">
          <label>Цена за день (₽) *</label>
          <input v-model.number="form.price" type="number" required min="0" step="100">
        </div>
        <div class="form-group">
          <label>Фото</label>
          <input type="file" accept="image/*" @change="onFileChange">
        </div>
        <div class="form-actions">
          <NuxtLink :to="isNew ? '/surfaces' : `/surfaces/${id}`" class="btn btn-secondary">Отмена</NuxtLink>
          <button type="submit" class="btn btn-primary">Сохранить</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'

const route = useRoute()
const id = computed(() => Number(route.params.id) || null)
const isNew = computed(() => !id.value)

const form = ref({
  name: '',
  address: '',
  type: 'Билборд 3x6',
  price: 5000
})

const surfaces = ref([
  { id: 1, name: 'Билборд Тверская 15', address: 'г. Москва, ул. Тверская, д.15', type: 'Билборд 3x6', price: 5000 },
  { id: 2, name: 'Ситилайт Арбат 10', address: 'г. Москва, ул. Арбат, д.10', type: 'Ситилайт 1.2x1.8', price: 3000 },
  { id: 3, name: 'Видеоэкран Садовая 5', address: 'г. Москва, ул. Садовая, д.5', type: 'Видеоэкран', price: 8000 },
  { id: 4, name: 'Билборд Ленинский 20', address: 'г. Москва, Ленинский пр-т, д.20', type: 'Билборд 3x6', price: 5500 },
  { id: 5, name: 'Баннер МКАД 45 км', address: 'МКАД, 45-й км, внешняя сторона', type: 'Баннер 3x12', price: 7000 }
])

watch(id, (newId) => {
  if (newId) {
    const s = surfaces.value.find(s => s.id === newId)
    if (s) {
      form.value = { name: s.name, address: s.address, type: s.type, price: s.price }
    }
  } else {
    form.value = { name: '', address: '', type: 'Билборд 3x6', price: 5000 }
  }
}, { immediate: true })

function onFileChange() {}

function submit() {
  if (id.value) {
    const s = surfaces.value.find(s => s.id === id.value)
    if (s) Object.assign(s, form.value)
    navigateTo(`/surfaces/${id.value}`)
  } else {
    navigateTo('/surfaces')
  }
}
</script>

<style scoped>
.edit-page { max-width: 600px; }
.back-link { color: #1e3c72; text-decoration: none; margin-bottom: 0.5rem; display: inline-block; }
.back-link:hover { text-decoration: underline; }
.page-header { margin-bottom: 1rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; }
.tabs { display: flex; gap: 0.25rem; margin-bottom: 1.5rem; border-bottom: 2px solid #e5e7eb; }
.tab { padding: 0.75rem 1.25rem; color: #6b7280; text-decoration: none; font-weight: 500; border-bottom: 2px solid transparent; margin-bottom: -2px; }
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }
.form-card { padding: 2rem; }
.form-group { margin-bottom: 1.5rem; }
.form-group label { display: block; margin-bottom: 0.5rem; color: #1a1a2e; font-weight: 500; }
.form-group input, .form-group select { width: 100%; padding: 0.75rem; border: 1px solid #d1d5db; border-radius: 8px; font-size: 1rem; }
.form-group input:focus, .form-group select:focus { outline: none; border-color: #1e3c72; }
.form-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 2rem; }
</style>
