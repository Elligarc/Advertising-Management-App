<template>
  <div class="edit-page">
    <div class="page-header">
      <NuxtLink to="/surfaces" class="back-link">К списку поверхностей</NuxtLink>
      <h1 class="page-title">Редактирование поверхности</h1>
    </div>

    <nav class="tabs">
      <NuxtLink to="/surfaces" class="tab">Список</NuxtLink>
      <NuxtLink to="/surfaces/add" class="tab">Добавить поверхность</NuxtLink>
      <NuxtLink :to="`/surfaces/${id}`" class="tab">Просмотр</NuxtLink>
    </nav>

    <div v-if="pending" class="state-msg">Загрузка...</div>
    <div v-else-if="fetchError" class="state-msg error-msg">Ошибка загрузки поверхности</div>

    <template v-else>

      <!-- Основные параметры -->
      <div class="form-card card">
        <h2 class="section-title">Параметры показа</h2>
        <p v-if="errors.main" class="error-msg">{{ errors.main }}</p>
        <form @submit.prevent="submitMain">
          <div class="form-row">
            <div class="form-group">
              <label>Длительность петли (сек)</label>
              <input v-model.number="mainForm.loopDuration" type="number" min="0" placeholder="Например: 60">
            </div>
            <div class="form-group">
              <label>Длительность слота (сек)</label>
              <input v-model.number="mainForm.slotDuration" type="number" min="0" placeholder="Например: 10">
            </div>
          </div>
          <div class="form-actions">
            <button type="submit" class="btn btn-primary" :disabled="loadings.main">
              {{ loadings.main ? 'Сохранение...' : 'Сохранить параметры' }}
            </button>
          </div>
        </form>
      </div>

      <!-- Цена -->
      <div class="form-card card">
        <h2 class="section-title">Цена</h2>
        <p v-if="errors.price" class="error-msg">{{ errors.price }}</p>
        <form @submit.prevent="submitPrice">
          <div class="form-row">
            <div class="form-group">
              <label>Цена *</label>
              <input v-model.number="priceForm.price" type="number" required min="0" step="0.01">
            </div>
            <div class="form-group">
              <label>Тип цены *</label>
              <select v-model="priceForm.priceType" required>
                <option value="PerMonth">За месяц</option>
                <option value="PerShow">За показ</option>
              </select>
            </div>
          </div>
          <div class="form-group">
            <label>Действует с</label>
            <input v-model="priceForm.dateFrom" type="datetime-local">
          </div>
          <div class="form-actions">
            <button type="submit" class="btn btn-primary" :disabled="loadings.price">
              {{ loadings.price ? 'Сохранение...' : 'Обновить цену' }}
            </button>
          </div>
        </form>
      </div>

      <!-- Статус -->
      <div class="form-card card">
        <h2 class="section-title">Статус</h2>
        <p v-if="errors.status" class="error-msg">{{ errors.status }}</p>
        <form @submit.prevent="submitStatus">
          <div class="form-row">
            <div class="form-group">
              <label>Статус *</label>
              <select v-model="statusForm.status" required>
                <option value="Created">Создана</option>
                <option value="Decommissioned">Выведена из эксплуатации</option>
                <option value="UnderRepair">На ремонте</option>
              </select>
            </div>
            <div class="form-group">
              <label>Действует с *</label>
              <input v-model="statusForm.dateFrom" type="datetime-local" required>
            </div>
          </div>
          <div class="form-actions">
            <button type="submit" class="btn btn-primary" :disabled="loadings.status">
              {{ loadings.status ? 'Сохранение...' : 'Обновить статус' }}
            </button>
          </div>
        </form>
      </div>

    </template>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useSurfaces } from '~/composable/useSurfaces'

const route = useRoute()
const id = Number(route.params.id)

const mainForm = ref({ loopDuration: null, slotDuration: null })
const priceForm = ref({ price: 0, priceType: 'PerMonth', dateFrom: null })
const statusForm = ref({ status: 'Created', dateFrom: '' })

const loadings = ref({ main: false, price: false, status: false })
const errors = ref({ main: null, price: null, status: null })

const { fetchSurfaceById, updateSurface, updateSurfacePrice, updateSurfaceStatus } = useSurfaces()

// Загрузка данных поверхности
const surface = await fetchSurfaceById(id)
if (surface) {
  mainForm.value = {
    loopDuration: surface.loopDuration ?? null,
    slotDuration: surface.slotDuration ?? null,
  }
  priceForm.value = {
    price: surface.currentPrice ?? 0,
    priceType: surface.currentPriceType ?? 'PerMonth',
    dateFrom: null,
  }
  statusForm.value = {
    status: surface.currentStatus ?? 'Created',
    dateFrom: '',
  }
}

async function submitMain() {
  loadings.value.main = true
  errors.value.main = null
  try {
    await updateSurface(id, {
      loopDuration: mainForm.value.loopDuration || null,
      slotDuration: mainForm.value.slotDuration || null,
    })
    navigateTo(`/surfaces/${id}`)
  } catch {
    errors.value.main = 'Ошибка при сохранении параметров'
  } finally {
    loadings.value.main = false
  }
}

async function submitPrice() {
  loadings.value.price = true
  errors.value.price = null
  try {
    await updateSurfacePrice(id, {
      price: priceForm.value.price,
      priceType: priceForm.value.priceType,
      dateFrom: priceForm.value.dateFrom || null,
    })
    navigateTo(`/surfaces/${id}`)
  } catch {
    errors.value.price = 'Ошибка при обновлении цены'
  } finally {
    loadings.value.price = false
  }
}

async function submitStatus() {
  loadings.value.status = true
  errors.value.status = null
  try {
    await updateSurfaceStatus(id, {
      status: statusForm.value.status,
      dateFrom: statusForm.value.dateFrom,
    })
    navigateTo(`/surfaces/${id}`)
  } catch {
    errors.value.status = 'Ошибка при обновлении статуса'
  } finally {
    loadings.value.status = false
  }
}
</script>

<style scoped>
.edit-page { max-width: 640px; }
.back-link { color: #1e3c72; text-decoration: none; margin-bottom: 0.5rem; display: inline-block; }
.back-link:hover { text-decoration: underline; }
.page-header { margin-bottom: 1rem; }
.page-title { font-size: 1.75rem; color: #1a1a2e; font-weight: 600; }
.tabs { display: flex; gap: 0.25rem; margin-bottom: 1.5rem; border-bottom: 2px solid #e5e7eb; }
.tab { padding: 0.75rem 1.25rem; color: #6b7280; text-decoration: none; font-weight: 500; border-bottom: 2px solid transparent; margin-bottom: -2px; }
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }
.form-card { padding: 1.5rem 2rem 2rem; margin-bottom: 1.5rem; }
.section-title { font-size: 1.1rem; font-weight: 600; color: #1a1a2e; margin-bottom: 1.25rem; padding-bottom: 0.5rem; border-bottom: 1px solid #e5e7eb; }
.form-row { display: flex; gap: 1rem; }
.form-row .form-group { flex: 1; }
.form-group { margin-bottom: 1.25rem; }
.form-group label { display: block; margin-bottom: 0.5rem; color: #1a1a2e; font-weight: 500; }
.form-group input, .form-group select { width: 100%; padding: 0.75rem; border: 1px solid #d1d5db; border-radius: 8px; font-size: 1rem; box-sizing: border-box; }
.form-group input:focus, .form-group select:focus { outline: none; border-color: #1e3c72; }
.form-actions { display: flex; justify-content: flex-end; margin-top: 0.5rem; }
.state-msg { padding: 2rem; text-align: center; color: #6b7280; }
.error-msg { color: #e53e3e; background: #fff5f5; border: 1px solid #feb2b2; border-radius: 8px; padding: 0.75rem 1rem; margin-bottom: 1rem; }
button:disabled { opacity: 0.6; cursor: not-allowed; }
</style>