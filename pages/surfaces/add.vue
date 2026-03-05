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
      <p v-if="error" class="error-msg">{{ error }}</p>

      <form @submit.prevent="submit">

        <div class="form-group">
          <label>Выбор конструкции</label>
          <div class="construction-options">
            <label class="radio-option">
              <input type="radio" v-model="showNewConstruction" :value="false">
              <span>Существующая конструкция</span>
            </label>
            <label class="radio-option">
              <input type="radio" v-model="showNewConstruction" :value="true">
              <span>Создать новую конструкцию</span>
            </label>
          </div>
        </div>

        <!-- Существующая конструкция -->
        <div v-if="!showNewConstruction" class="form-group">
          <label>Конструкция *</label>
          <select v-model="form.constructionId" required :disabled="constructionsPending">
            <option value="" disabled>{{ constructionsPending ? 'Загрузка...' : 'Выберите конструкцию' }}</option>
            <option v-for="c in constructions" :key="c.id" :value="c.id">
              {{ c.address }} ({{ c.city?.name }}, {{ c.district?.name }})
            </option>
          </select>
        </div>

        <!-- Новая конструкция -->
        <div v-if="showNewConstruction" class="construction-form">
          <div class="form-row">
            <div class="form-group">
              <label>Город *</label>
              <select v-model="form.newConstruction.cityId" required>
                <option value="">Выберите город</option>
                <option v-for="city in cities" :key="city.id" :value="city.id">
                  {{ city.name }}
                </option>
              </select>
            </div>
            <div class="form-group">
              <label>Район *</label>
              <select v-model="form.newConstruction.districtId" required :disabled="!form.newConstruction.cityId">
                <option value="">Выберите район</option>
                <option v-for="district in filteredDistricts" :key="district.id" :value="district.id">
                  {{ district.name }}
                </option>
              </select>
            </div>
          </div>
          <div class="form-row">
            <div class="form-group">
              <label>Формат *</label>
              <select v-model="form.newConstruction.formatId" required>
                <option value="">Выберите формат</option>
                <option v-for="format in formats" :key="format.id" :value="format.id">
                  {{ format.name }} ({{ format.constructionType }})
                </option>
              </select>
            </div>
            <div class="form-group">
              <label>Адрес *</label>
              <input v-model="form.newConstruction.address" type="text" required placeholder="Введите адрес">
            </div>
          </div>
        </div>

        <div class="form-group">
          <label>Сторона *</label>
          <select v-model="form.side" required>
            <option value="A">A</option>
            <option value="B">B</option>
            <option value="C">C</option>
          </select>
        </div>

        <div class="form-group">
          <label>Тип поверхности *</label>
          <select v-model="form.surfaceType" required>
            <option value="Digital">Цифровая</option>
            <option value="Regular">Статичная</option>
          </select>
        </div>

        <template v-if="form.surfaceType === 'Digital'">
          <div class="form-row">
            <div class="form-group">
              <label>Длительность петли (сек)</label>
              <input v-model.number="form.loopDuration" type="number" min="0" placeholder="Например: 60">
            </div>
            <div class="form-group">
              <label>Длительность слота (сек)</label>
              <input v-model.number="form.slotDuration" type="number" min="0" placeholder="Например: 10">
            </div>
          </div>
        </template>

        <div class="form-row">
          <div class="form-group">
            <label>Начальная цена (₽) *</label>
            <input v-model.number="form.initialPrice" type="number" required min="0" step="100">
          </div>
          <div class="form-group">
            <label>Тип цены *</label>
            <select v-model="form.priceType" required>
              <option value="PerMonth">За месяц</option>
              <option value="PerShow">За показ</option>
            </select>
          </div>
        </div>

        <div class="form-actions">
          <NuxtLink to="/surfaces" class="btn">Отмена</NuxtLink>
          <button type="submit" class="btn btn-primary" :disabled="loading">
            {{ loading ? 'Сохранение...' : 'Сохранить' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useCities } from '~/composable/useCities'
import { useDistricts } from '~/composable/useDistricts'
import { useFormats } from '~/composable/useFormats'
import { useSurfaces } from '~/composable/useSurfaces'

const form = ref({
  constructionId: '',
  side: 'A',
  surfaceType: 'Regular',
  loopDuration: null,
  slotDuration: null,
  initialPrice: 5000,
  priceType: 'PerMonth',
  newConstruction: {
    address: '',
    cityId: '',
    districtId: '',
    formatId: ''
  }
})

const loading = ref(false)
const error = ref(null)
const showNewConstruction = ref(false)

const { cities } = useCities()
const { districts, getDistrictsByCity } = useDistricts()
const { formats } = useFormats()
const { createSurface } = useSurfaces()

// Загрузка конструкций
const apiBase = useRuntimeConfig().public.apiBase
const { data: constructions, pending: constructionsPending } = await useFetch(`${apiBase}/api/Constructions`, {
  default: () => []
})

const filteredDistricts = computed(() => {
  if (!form.value.newConstruction.cityId) {
    return []
  }
  return getDistrictsByCity(form.value.newConstruction.cityId)
})

async function submit() {
  loading.value = true
  error.value = null

  try {
    let constructionId = form.value.constructionId

    // Если выбрано создание новой конструкции
    if (showNewConstruction.value && form.value.newConstruction.address) {
      const newConstruction = await $fetch(`${apiBase}/api/Constructions`, {
        method: 'POST',
        body: {
          address: form.value.newConstruction.address,
          districtId: form.value.newConstruction.districtId,
          formatId: form.value.newConstruction.formatId
        }
      })
      constructionId = newConstruction.id
    }

    await $fetch(`${apiBase}/api/Surfaces`, {
      method: 'POST',
      body: {
        constructionId: constructionId,
        side: form.value.side,
        surfaceType: form.value.surfaceType,
        loopDuration: form.value.surfaceType === 'Digital' ? form.value.loopDuration || null : null,
        slotDuration: form.value.surfaceType === 'Digital' ? form.value.slotDuration || null : null,
        initialPrice: form.value.initialPrice,
        priceType: form.value.priceType,
      }
    })

    navigateTo('/surfaces')
  } catch (err) {
    console.error('Error creating surface:', err)
    error.value = 'Ошибка при создании поверхности. Попробуйте ещё раз.'
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
.form-group input, .form-group select {
  width: 100%; padding: 0.75rem; border: 2px solid #e0e0e0; border-radius: 8px; font-size: 1rem; box-sizing: border-box;
}
.form-group input:focus, .form-group select:focus { outline: none; border-color: #2a5298; }
.form-row { display: flex; gap: 1rem; }
.form-row .form-group { flex: 1; }
.form-actions { display: flex; gap: 1rem; justify-content: flex-end; margin-top: 2rem; }
.tabs { display: flex; gap: 0.25rem; margin-bottom: 1.5rem; border-bottom: 2px solid #e5e7eb; }
.tab { padding: 0.75rem 1.25rem; color: #6b7280; text-decoration: none; font-weight: 500; border-bottom: 2px solid transparent; margin-bottom: -2px; }
.tab:hover { color: #1e3c72; }
.tab--active { color: #1e3c72; border-bottom-color: #1e3c72; }
.construction-options {
  display: flex;
  gap: 1rem;
  margin-top: 0.5rem;
}

.radio-option {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 0.75rem;
  border: 1px solid #e0e0e0;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s;
}

.radio-option:hover {
  border-color: #2a5298;
  background: #f8fafc;
}

.radio-option input {
  margin: 0;
}

.construction-form {
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  padding: 1.5rem;
  background: #f9fafb;
  margin-top: 1rem;
}

.error-msg { color: #e53e3e; background: #fff5f5; border: 1px solid #feb2b2; border-radius: 8px; padding: 0.75rem 1rem; margin-bottom: 1rem; }
button:disabled { opacity: 0.6; cursor: not-allowed; }
</style>