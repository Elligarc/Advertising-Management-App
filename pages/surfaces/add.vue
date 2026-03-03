<template>
  <div class="add-page">
    <div class="page-header">
      <h1 class="page-title">Новая рекламная поверхность</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/surfaces" class="tab">Список</NuxtLink>
      <NuxtLink to="/surfaces/add" class="tab tab--active">Добавить поверхность</NuxtLink>
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
          <NuxtLink to="/surfaces" class="btn">Отмена</NuxtLink>
          <button type="submit" class="btn btn-primary">Сохранить</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'

const form = ref({
  name: '',
  address: '',
  type: 'Билборд 3x6',
  price: 5000
})

function onFileChange() {
  // Загрузка файла можно подключить к API
}

function submit() {
  // В реальном приложении — вызов API. Пока редирект на список.
  navigateTo('/surfaces')
}
</script>

<style scoped>
.add-page { max-width: 600px; }
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
.form-group select {
  width: 100%;
  padding: 0.75rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
}

.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #2a5298;
}

.form-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 2rem; }

.tabs { display: flex; gap: 0.25rem; margin-bottom: 1.5rem; border-bottom: 2px solid #e5e7eb; }
.tab { padding: 0.75rem 1.25rem; color: #6b7280; text-decoration: none; font-weight: 500; border-bottom: 2px solid transparent; margin-bottom: -2px; }
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }
</style>
