<template>
  <div class="edit-page">
    <div class="page-header">
      <NuxtLink to="/clients" class="back-link">← К списку клиентов</NuxtLink>
      <h1 class="page-title">Редактирование клиента</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/clients" class="tab">Список</NuxtLink>
      <NuxtLink to="/clients/add" class="tab">Добавить клиента</NuxtLink>
      <NuxtLink :to="`/clients/${id}`" class="tab">Просмотр</NuxtLink>
    </nav>

    <div v-if="pending" class="state-msg">Загрузка...</div>
    <div v-else-if="fetchError" class="state-msg error-msg">Ошибка загрузки клиента</div>

    <div v-else class="form-card card">
      <p v-if="submitError" class="error-msg">{{ submitError }}</p>

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
          <NuxtLink :to="`/clients/${id}`" class="btn btn-secondary">Отмена</NuxtLink>
          <button type="submit" class="btn btn-primary" :disabled="saving">
            {{ saving ? 'Сохранение...' : 'Сохранить' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
const route = useRoute()
const id = Number(route.params.id)
const API = 'http://localhost:5000'

const form = ref({ name: '', phone: '' })
const saving = ref(false)
const submitError = ref(null)

const { pending, error: fetchError } = await useFetch(`${API}/api/Clients/${id}`, {
  onResponse({ response }) {
    form.value.name = response._data.name ?? ''
    form.value.phone = response._data.phone ?? ''
  }
})

async function submit() {
  saving.value = true
  submitError.value = null
  try {
    await $fetch(`${API}/api/Clients/${id}`, {
      method: 'PUT',
      body: { name: form.value.name, phone: form.value.phone }
    })
    navigateTo(`/clients/${id}`)
  } catch {
    submitError.value = 'Ошибка при сохранении. Попробуйте ещё раз.'
  } finally {
    saving.value = false
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
.form-group input { width: 100%; padding: 0.75rem; border: 1px solid #d1d5db; border-radius: 8px; font-size: 1rem; box-sizing: border-box; }
.form-group input:focus { outline: none; border-color: #1e3c72; }
.form-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 2rem; }
.state-msg { padding: 2rem; text-align: center; color: #6b7280; }
.error-msg { color: #e53e3e; background: #fff5f5; border: 1px solid #feb2b2; border-radius: 8px; padding: 0.75rem 1rem; margin-bottom: 1rem; }
button:disabled { opacity: 0.6; cursor: not-allowed; }
</style>