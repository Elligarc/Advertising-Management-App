<template>
  <div class="client-card card">
    <div class="card-header">
      <h3>{{ client.name || 'Без названия' }}</h3>
    </div>
    <div class="card-body">
      <p class="phone"><span class="label">Телефон:</span> {{ client.phone || 'Не указан' }}</p>
    </div>
    <div class="card-footer">
      <NuxtLink :to="`/clients/${client.id}`" class="btn btn-primary btn-sm">Карточка</NuxtLink>
      <NuxtLink :to="`/clients/${client.id}/edit`" class="btn btn-secondary btn-sm">Редактировать</NuxtLink>
      <button class="btn btn-success btn-sm" @click="navigateTo(`/surfaces?rentClient=${client.id}`)">Новая аренда</button>
    </div>
  </div>
</template>

<script setup>
defineProps({
  client: {
    type: Object,
    required: true
  }
})
</script>

<style scoped>
.client-card { display: flex; flex-direction: column; }
.card-header { padding: 1.5rem 1.5rem 0.5rem; }
.card-header h3 { margin: 0; color: #1a1a2e; font-size: 1.1rem; font-weight: 600; }
.card-body { padding: 0.5rem 1.5rem; flex: 1; }
.card-body p { margin: 0.35rem 0; color: #4b5563; font-size: 0.95rem; }
.label { font-weight: 500; color: #6b7280; margin-right: 0.25rem; }
.card-footer { display: flex; gap: 0.5rem; flex-wrap: wrap; padding: 1rem 1.5rem; border-top: 1px solid #e5e7eb; }
.btn-sm { padding: 0.5rem 1rem; font-size: 0.875rem; }
</style>