<template>
  <div class="client-detail">
    <div class="page-header">
      <NuxtLink to="/clients" class="back-link">К списку клиентов</NuxtLink>
      <h1 class="page-title">{{ client?.name || 'Клиент' }}</h1>
      <NuxtLink v-if="client" :to="`/clients/${id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
    </div>

    <div v-if="pending" class="state-msg">Загрузка...</div>
    <div v-else-if="error" class="state-msg error-msg">Ошибка загрузки клиента</div>

    <div v-else-if="client" class="detail-card card">
      <div class="detail-row">
        <span class="label">Телефон</span>
        <span class="value">{{ client.phone }}</span>
      </div>
      <div class="actions">
        <NuxtLink :to="`/clients/${id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
        <button class="btn btn-success" @click="navigateTo(`/surfaces?rentClient=${id}`)">Новая аренда</button>
      </div>
    </div>

    <p v-else class="empty">Клиент не найден.</p>
  </div>
</template>

<script setup>
const route = useRoute()
const id = Number(route.params.id)
const API = 'http://localhost:5000'

const { data: client, pending, error } = await useFetch(`${API}/api/Clients/${id}`)
</script>

<style scoped>
.client-detail { max-width: 720px; }
.back-link { color: #1e3c72; text-decoration: none; font-size: 0.95rem; margin-bottom: 0.5rem; display: inline-block; }
.back-link:hover { text-decoration: underline; }
.page-header { display: flex; flex-wrap: wrap; align-items: center; gap: 1rem; margin-bottom: 1.5rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; flex: 1; }
.detail-card { padding: 1.5rem; }
.detail-row { display: flex; padding: 0.75rem 0; border-bottom: 1px solid #e5e7eb; gap: 1rem; }
.detail-row .label { color: #6b7280; min-width: 160px; }
.detail-row .value { color: #1a1a2e; font-weight: 500; }
.actions { display: flex; gap: 0.75rem; margin-top: 1.5rem; flex-wrap: wrap; }
.state-msg { padding: 2rem; text-align: center; color: #6b7280; }
.error-msg { color: #e53e3e; background: #fff5f5; border-radius: 8px; }
.empty { color: #6b7280; }
</style>