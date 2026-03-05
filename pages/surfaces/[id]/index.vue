<template>
  <div class="surface-detail">
    <div class="page-header">
      <NuxtLink to="/surfaces" class="back-link">К списку поверхностей</NuxtLink>
      <h1 class="page-title">{{ surface?.construction?.address || 'Поверхность' }}</h1>
      <NuxtLink v-if="surface" :to="`/surfaces/${id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
    </div>

    <div v-if="pending" class="state-msg">Загрузка...</div>
    <div v-else-if="error" class="state-msg error-msg">Ошибка загрузки поверхности</div>

    <div v-else-if="surface" class="detail-card card">
      <div class="detail-row">
        <span class="label">Адрес</span>
        <span class="value">{{ surface.construction?.address }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Город</span>
        <span class="value">{{ surface.construction?.city?.name }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Район</span>
        <span class="value">{{ surface.construction?.district?.name }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Тип конструкции</span>
        <span class="value">{{ surface.construction?.format?.name }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Сторона</span>
        <span class="value">{{ surface.side }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Тип поверхности</span>
        <span class="value">{{ surface.surfaceType === 'Digital' ? 'Цифровая' : 'Статичная' }}</span>
      </div>
      <div v-if="surface.loopDuration" class="detail-row">
        <span class="label">Длительность петли</span>
        <span class="value">{{ surface.loopDuration }} сек</span>
      </div>
      <div v-if="surface.slotDuration" class="detail-row">
        <span class="label">Длительность слота</span>
        <span class="value">{{ surface.slotDuration }} сек</span>
      </div>
      <div class="detail-row">
        <span class="label">Макс. слотов</span>
        <span class="value">{{ surface.maxSlots }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Цена</span>
        <span class="value">{{ surface.currentPrice }} ₽ / {{ surface.currentPriceType === 'PerMonth' ? 'мес' : 'показ' }}</span>
      </div>
      <div class="detail-row">
        <span class="label">Статус</span>
        <span :class="['status', statusClass(surface.currentStatus)]">{{ statusText(surface.currentStatus) }}</span>
      </div>

      <div class="actions">
        <NuxtLink :to="`/surfaces/${id}/edit`" class="btn btn-primary">Редактировать</NuxtLink>
        <button v-if="surface.currentStatus === 'Created'" class="btn btn-success" @click="navigateTo(`/surfaces/rent/${id}`)">Сдать в аренду</button>
      </div>
    </div>

    <p v-else class="empty">Поверхность не найдена.</p>
  </div>
</template>

<script setup>
import { useSurfaces } from '~/composable/useSurfaces'

const route = useRoute()
const id = Number(route.params.id)

const { fetchSurfaceById, loading: pending, error } = useSurfaces()

// Загрузка поверхности по ID
const surface = await fetchSurfaceById(id)

function statusClass(status) {
  return { Created: 'status-free', UnderRepair: 'status-repair', Decommissioned: 'status-busy' }[status] ?? ''
}

function statusText(status) {
  return { Created: 'Активна', UnderRepair: 'На ремонте', Decommissioned: 'Выведена' }[status] ?? status
}
</script>

<style scoped>
.surface-detail { max-width: 720px; }
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