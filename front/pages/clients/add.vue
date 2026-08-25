<template>
  <div class="add-page">
    <div class="page-header">
      <h1 class="page-title">Новый клиент</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/clients" class="tab">Список</NuxtLink>
      <NuxtLink to="/clients/add" class="tab tab--active">Добавить клиента</NuxtLink>
    </nav>

    <div class="form-card card">
      <p v-if="error" class="error-msg">{{ error }}</p>

      <form @submit.prevent="submit">
        <div class="form-group">
          <label>Название компании *</label>
          <input v-model="form.name" type="text" required placeholder="ООО «Название»">
        </div>
        <div class="form-group">
          <label>Телефон *</label>
          <input v-model="form.phone" type="tel" required placeholder="+7 (999) 123-45-67">
        </div>
        <div class="form-actions">
          <NuxtLink to="/clients" class="btn">Отмена</NuxtLink>
          <button type="submit" class="btn btn-primary" :disabled="loading">
            {{ loading ? 'Сохранение...' : 'Сохранить' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
const API = 'http://localhost:5000'
const form = ref({ name: '', phone: '' })
const loading = ref(false)
const error = ref(null)

async function submit() {
  loading.value = true
  error.value = null
  try {
    await $fetch(`${API}/api/Clients`, {
      method: 'POST',
      body: { name: form.value.name, phone: form.value.phone }
    })
    navigateTo('/clients')
  } catch {
    error.value = 'Ошибка при создании клиента. Попробуйте ещё раз.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.add-page { max-width: 600px; }
.page-title { font-size: 1.75rem; color: #2c3e50; margin-bottom: 1.5rem; }
.form-card { padding: 2rem; }
.form-group { margin-bottom: 1.5rem; }
.form-group label { display: block; margin-bottom: 0.5rem; color: #2c3e50; font-weight: 500; }
.form-group input, .form-group textarea {
  width: 100%; padding: 0.75rem; border: 2px solid #e0e0e0; border-radius: 8px; font-size: 1rem; box-sizing: border-box;
}
.form-group input:focus { outline: none; border-color: #2a5298; }
.form-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 2rem; }
.tabs { display: flex; gap: 0.25rem; margin-bottom: 1.5rem; border-bottom: 2px solid #e5e7eb; }
.tab { padding: 0.75rem 1.25rem; color: #6b7280; text-decoration: none; font-weight: 500; border-bottom: 2px solid transparent; margin-bottom: -2px; }
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }
.error-msg { color: #e53e3e; background: #fff5f5; border: 1px solid #feb2b2; border-radius: 8px; padding: 0.75rem 1rem; margin-bottom: 1rem; }
button:disabled { opacity: 0.6; cursor: not-allowed; }
</style>