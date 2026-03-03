<template>
  <div class="edit-page">
    <div class="page-header">
      <NuxtLink to="/clients" class="back-link">К списку клиентов</NuxtLink>
      <h1 class="page-title">{{ isNew ? 'Новый клиент' : 'Редактирование клиента' }}</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/clients" class="tab">Список</NuxtLink>
      <NuxtLink to="/clients/add" class="tab">Добавить клиента</NuxtLink>
      <NuxtLink v-if="!isNew && id" :to="`/clients/${id}`" class="tab">Просмотр</NuxtLink>
    </nav>

    <div class="form-card card">
      <form @submit.prevent="submit">
        <div class="form-group">
          <label>Название компании *</label>
          <input v-model="form.name" type="text" required placeholder="ООО «Название»">
        </div>
        <div class="form-group">
          <label>Контактное лицо</label>
          <input v-model="form.contactPerson" type="text" placeholder="ФИО">
        </div>
        <div class="form-group">
          <label>Телефон *</label>
          <input v-model="form.phone" type="tel" required placeholder="+7 (999) 123-45-67">
        </div>
        <div class="form-group">
          <label>Email</label>
          <input v-model="form.email" type="email" placeholder="email@company.ru">
        </div>
        <div class="form-group">
          <label>ИНН</label>
          <input v-model="form.inn" type="text" placeholder="10 или 12 цифр">
        </div>
        <div class="form-group">
          <label>Адрес</label>
          <input v-model="form.address" type="text" placeholder="Юридический адрес">
        </div>
        <div class="form-group">
          <label>Примечание</label>
          <textarea v-model="form.notes" rows="3" placeholder="Комментарий"></textarea>
        </div>
        <div class="form-actions">
          <NuxtLink :to="isNew ? '/clients' : `/clients/${id}`" class="btn btn-secondary">Отмена</NuxtLink>
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
  contactPerson: '',
  phone: '',
  email: '',
  inn: '',
  address: '',
  notes: ''
})

const clients = ref([
  { id: 1, name: 'ООО "Ромашка"', contactPerson: 'Иванов Иван Иванович', phone: '+7 (999) 123-45-67', email: 'ivan@romashka.ru', inn: '7712345678', address: 'г. Москва, ул. Цветной бульвар, д.10', notes: '' },
  { id: 2, name: 'ООО "ТехноПлюс"', contactPerson: 'Петров Петр Петрович', phone: '+7 (999) 765-43-21', email: 'petrov@techno.ru', inn: '7723456789', address: 'г. Москва, ул. Новый Арбат, д.5', notes: '' },
  { id: 3, name: 'ИП Сидоров', contactPerson: 'Сидоров Сидор', phone: '+7 (999) 111-22-33', email: 'sidorov@mail.ru', inn: '7734567890', address: 'г. Москва, ул. Ленина, д.1', notes: '' }
])

watch(id, (newId) => {
  if (newId) {
    const c = clients.value.find(c => c.id === newId)
    if (c) {
      form.value = {
        name: c.name,
        contactPerson: c.contactPerson || '',
        phone: c.phone || '',
        email: c.email || '',
        inn: c.inn || '',
        address: c.address || '',
        notes: c.notes || ''
      }
    }
  } else {
    form.value = { name: '', contactPerson: '', phone: '', email: '', inn: '', address: '', notes: '' }
  }
}, { immediate: true })

function submit() {
  if (id.value) {
    const c = clients.value.find(c => c.id === id.value)
    if (c) Object.assign(c, form.value)
    navigateTo(`/clients/${id.value}`)
  } else {
    navigateTo('/clients')
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
.form-group input, .form-group select, .form-group textarea { width: 100%; padding: 0.75rem; border: 1px solid #d1d5db; border-radius: 8px; font-size: 1rem; }
.form-group input:focus, .form-group select:focus, .form-group textarea:focus { outline: none; border-color: #1e3c72; }
.form-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 2rem; }
</style>
