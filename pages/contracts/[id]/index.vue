<template>
  <div class="contract-details-page">
    <div class="page-header">
      <div class="contract-info">
        <h1>Договор №{{ contract.id }}</h1>
        <div class="client-info">
          <span class="label">Клиент:</span>
          <span class="value">{{ contract.clientName }}</span>
        </div>
        <div class="contract-dates">
          <span class="label">С:</span>
          <span class="value">{{ formatDate(contract.startDate) }}</span>
          <span class="label">По:</span>
          <span class="value">{{ formatDate(contract.endDate) }}</span>
        </div>
      </div>
      <div class="contract-actions">
        <span class="status" :class="contract.status.toLowerCase()">
          {{ getStatusText(contract.status) }}
        </span>
        <span class="total-price">
          Сумма: {{ formatPrice(contract.totalPrice) }}
        </span>
      </div>
    </div>

    <div v-if="loading" class="loading">
      Загрузка...
    </div>

    <div v-if="error" class="error">
      {{ error }}
    </div>

    <!-- Форма добавления поверхности -->
    <div class="add-surface-section">
      <h2>Добавить поверхность</h2>
      <form @submit.prevent="addSurfaceToContract" class="surface-form">
        <div class="form-row">
          <div class="form-group">
            <label for="surface">Поверхность</label>
            <select 
              id="surface" 
              v-model="surfaceForm.surfaceId" 
              required
              class="form-control"
            >
              <option value="">Выберите поверхность</option>
              <option 
                v-for="surface in availableSurfaces" 
                :key="surface.id" 
                :value="surface.id"
                :disabled="isSurfaceAlreadyAdded(surface.id)"
              >
                {{ getSurfaceLabel(surface) }}
              </option>
            </select>
          </div>
          
          <div class="form-group">
            <label for="startDate">Дата начала</label>
            <input 
              type="date" 
              id="startDate" 
              v-model="surfaceForm.startDate" 
              required
              class="form-control"
            >
          </div>
          
          <div class="form-group">
            <label for="endDate">Дата окончания</label>
            <input 
              type="date" 
              id="endDate" 
              v-model="surfaceForm.endDate" 
              required
              class="form-control"
            >
          </div>
        </div>

        <div class="form-row">
          <div class="form-group">
            <label for="price">Цена</label>
            <input 
              type="number" 
              id="price" 
              v-model.number="surfaceForm.price" 
              required
              min="0"
              step="0.01"
              class="form-control"
            >
          </div>
          
          <div class="form-group">
            <label for="priceType">Тип цены</label>
            <select 
              id="priceType" 
              v-model="surfaceForm.priceType" 
              required
              class="form-control"
            >
              <option value="PerShow">За показ</option>
              <option value="PerMonth">За месяц</option>
            </select>
          </div>
        </div>

        <div class="form-actions">
          <button type="submit" class="btn btn-primary" :disabled="loading">
            Добавить поверхность
          </button>
        </div>
      </form>
    </div>

    <!-- Список добавленных поверхностей -->
    <div v-if="contract.items && contract.items.length > 0" class="contract-items">
      <h2>Поверхности в договоре</h2>
      <div class="items-list">
        <div v-for="item in contract.items" :key="item.id" class="item-card">
          <div class="item-info">
            <div class="surface-details">
              <h4>{{ getSurfaceLabel(item.surface) }}</h4>
              <p class="construction-address">{{ item.surface.construction.address }}</p>
            </div>
            <div class="item-dates">
              <span class="date-label">С:</span>
              <span>{{ formatDate(item.startDate) }}</span>
              <span class="date-label">По:</span>
              <span>{{ formatDate(item.endDate) }}</span>
            </div>
          </div>
          <div class="item-footer">
            <span class="price">
              Цена: {{ formatPrice(item.price) }} ({{ getPriceTypeText(item.priceType) }})
            </span>
            <span class="total-price">
              Сумма: {{ formatPrice(item.totalPrice) }}
            </span>
          </div>
        </div>
      </div>
    </div>

    <div v-else class="empty-items">
      В договоре пока нет поверхностей
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useContracts } from '~/composable/useContracts'
import { useSurfaces } from '~/composable/useSurfaces'

const route = useRoute()
const contractId = route.params.id

const { contract, loading, error, getContract, addSurfaceToContract: addSurfaceToContractApi } = useContracts()
const { surfaces, fetchSurfaces } = useSurfaces()

const surfaceForm = ref({
  surfaceId: '',
  startDate: '',
  endDate: '',
  price: 0,
  priceType: 'PerShow'
})

onMounted(async () => {
  await Promise.all([
    getContract(contractId),
    fetchSurfaces()
  ])
})

const addSurfaceToContract = async () => {
  if (!surfaceForm.value.surfaceId || !surfaceForm.value.startDate || !surfaceForm.value.endDate || !surfaceForm.value.price) {
    alert('Пожалуйста, заполните все поля')
    return
  }

  try {
    await addSurfaceToContractApi(contractId, {
      surfaceId: parseInt(surfaceForm.value.surfaceId),
      startDate: surfaceForm.value.startDate,
      endDate: surfaceForm.value.endDate,
      price: parseFloat(surfaceForm.value.price),
      priceType: surfaceForm.value.priceType
    })
    
    // Обновляем данные договора
    await getContract(contractId)
    
    // Сбрасываем форму
    surfaceForm.value = {
      surfaceId: '',
      startDate: '',
      endDate: '',
      price: 0,
      priceType: 'PerShow'
    }
  } catch (err) {
    alert(err.message || 'Ошибка добавления поверхности')
  }
}

const availableSurfaces = computed(() => {
  if (!surfaces.value) return []
  if (!contract.value?.items) return surfaces.value
  
  const addedSurfaceIds = contract.value.items.map(item => item.surfaceId)
  return surfaces.value.filter(surface => !addedSurfaceIds.includes(surface.id))
})

const isSurfaceAlreadyAdded = (surfaceId) => {
  if (!contract.value?.items) return false
  return contract.value.items.some(item => item.surfaceId === surfaceId)
}

const getSurfaceLabel = (surface) => {
  return `${surface.construction.address}, ${surface.side} сторона, ${surface.surfaceType}`
}

const getPriceTypeText = (priceType) => {
  return priceType === 'PerShow' ? 'За показ' : 'За месяц'
}

const formatDate = (dateString) => {
  if (!dateString) return ''
  const date = new Date(dateString)
  return date.toLocaleDateString('ru-RU')
}

const formatPrice = (price) => {
  if (!price) return '0 ₽'
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 0
  }).format(price)
}

const getStatusText = (status) => {
  const statusMap = {
    'Created': 'Создан',
    'Active': 'Активен',
    'Cancelled': 'Отменен'
  }
  return statusMap[status] || status
}
</script>

<style scoped>
.contract-details-page {
  padding: 20px;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 20px;
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid #e0e0e0;
}

.contract-info h1 {
  margin: 0 0 10px 0;
  color: #333;
}

.client-info, .contract-dates {
  display: flex;
  gap: 10px;
  margin-bottom: 5px;
  font-size: 14px;
  color: #666;
}

.label {
  font-weight: bold;
  color: #333;
}

.value {
  color: #333;
}

.contract-actions {
  display: flex;
  flex-direction: column;
  gap: 10px;
  align-items: flex-end;
}

.status {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: bold;
  text-transform: uppercase;
}

.status.created {
  background-color: #fff3cd;
  color: #856404;
  border: 1px solid #ffeaa7;
}

.status.active {
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
}

.status.cancelled {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.total-price {
  font-weight: bold;
  color: #333;
  font-size: 16px;
}

.add-surface-section {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid #e0e0e0;
  margin-bottom: 20px;
}

.add-surface-section h2 {
  margin: 0 0 15px 0;
  color: #333;
  font-size: 18px;
}

.surface-form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-row {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 15px;
}

.form-group {
  display: flex;
  flex-direction: column;
}

.form-group label {
  margin-bottom: 5px;
  font-weight: 500;
  color: #333;
  font-size: 14px;
}

.form-control {
  padding: 8px 12px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 14px;
  transition: border-color 0.2s;
}

.form-control:focus {
  outline: none;
  border-color: #007bff;
  box-shadow: 0 0 0 2px rgba(0, 123, 255, 0.25);
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  margin-top: 10px;
}

.btn {
  padding: 8px 16px;
  border-radius: 4px;
  text-decoration: none;
  font-weight: 500;
  border: 1px solid transparent;
  cursor: pointer;
  transition: all 0.2s;
  font-size: 14px;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border-color: #007bff;
}

.btn-primary:hover:not(:disabled) {
  background-color: #0056b3;
  border-color: #0056b3;
}

.btn-primary:disabled {
  background-color: #6c757d;
  border-color: #6c757d;
  cursor: not-allowed;
}

.contract-items {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid #e0e0e0;
}

.contract-items h2 {
  margin: 0 0 15px 0;
  color: #333;
  font-size: 18px;
}

.items-list {
  display: grid;
  gap: 12px;
}

.item-card {
  border: 1px solid #e0e0e0;
  border-radius: 6px;
  padding: 12px;
  background: #f8f9fa;
}

.item-info {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 8px;
}

.surface-details h4 {
  margin: 0 0 4px 0;
  color: #333;
  font-size: 16px;
}

.construction-address {
  margin: 0;
  color: #666;
  font-size: 14px;
}

.item-dates {
  display: flex;
  gap: 8px;
  font-size: 12px;
  color: #888;
}

.date-label {
  font-weight: bold;
}

.item-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  font-size: 14px;
}

.price {
  color: #666;
}

.total-price {
  font-weight: bold;
  color: #333;
}

.empty-items {
  text-align: center;
  padding: 40px;
  color: #666;
  background: white;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid #e0e0e0;
}

.loading, .error {
  padding: 10px;
  margin-bottom: 20px;
  border-radius: 4px;
  text-align: center;
}

.loading {
  background-color: #e9ecef;
  color: #495057;
}

.error {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}
</style>