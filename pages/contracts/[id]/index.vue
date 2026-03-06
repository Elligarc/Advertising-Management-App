<template>
  <div class="contract-details-page">
    <!-- Проверка на наличие данных о договоре -->
    <div v-if="!contract" class="error">
      Договор не найден или данные не загружены
    </div>

    <div v-else>
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
          <button 
            v-if="contract.status === 'Created'" 
            @click="handleDeleteContract" 
            class="btn btn-danger"
          >
            Удалить
          </button>
        </div>
      </div>

      <div v-if="loading" class="loading">
        Загрузка данных договора...
      </div>

      <div v-if="error" class="error">
        <strong>Ошибка загрузки:</strong> {{ error }}
        <br>
        <small>Проверьте консоль разработчика для подробной информации</small>
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
        <ContractItem 
          v-for="item in contract.items" 
          :key="item.id" 
          :surface="item.surface"
          :start-date="item.startDate"
          :end-date="item.endDate"
          :price="item.price"
          :price-type="item.priceType"
          :total-price="item.totalPrice"
        />
      </div>
    </div>

    <div v-else class="empty-items">
      В договоре пока нет поверхностей
    </div>
  </div>
</div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useContracts } from '~/composable/useContracts'
import { useSurfaces } from '~/composable/useSurfaces'
import ContractItem from '~/components/ContractItem.vue'

const router = useRouter()

const route = useRoute()
const contractId = route.params.id

const { contracts, contractsLoading, contractsError, getContract, addContractItem, deleteContract } = useContracts()
const { surfaces, surfacesLoading, surfacesError, fetchSurfaces } = useSurfaces()

const surfaceForm = ref({
  surfaceId: '',
  startDate: '',
  endDate: '',
  price: 0,
  priceType: 'PerShow'
})

const contract = ref(null)
const pageLoading = ref(true)
const pageError = ref(null)

onMounted(async () => {
  console.log('=== Монтируем компонент деталей договора ===')
  console.log('ID договора из маршрута:', contractId)
  console.log('Тип contractId:', typeof contractId)
  console.log('Route params:', route.params)

  // Валидация ID
  if (!contractId || isNaN(parseInt(contractId))) {
    console.error('=== Ошибка: некорректный ID договора ===')
    pageError.value = 'Некорректный ID договора'
    pageLoading.value = false
    return
  }

  try {
    console.log('=== Начинаем загрузку данных ===')
    console.log('=== Загружаем договор ===')
    const loadedContract = await getContract(parseInt(contractId))
    console.log('=== Загружаем поверхности ===')
    await fetchSurfaces()
    console.log('=== Данные успешно загружены ===')
    console.log('Загруженный договор:', loadedContract)
    contract.value = loadedContract
    pageLoading.value = false
  } catch (err) {
    console.error('=== Ошибка при загрузке данных ===')
    console.error('Ошибка:', err)
    console.error('Тип ошибки:', err?.constructor?.name)
    console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))

    if (err?.response?.status === 404) {
      pageError.value = 'Договор не найден'
    } else {
      pageError.value = err instanceof Error ? err.message : 'Ошибка загрузки данных'
    }
    pageLoading.value = false
  }
})

const addSurfaceToContract = async () => {
  console.log('=== Добавляем поверхность к договору ===')
  console.log('FormData:', surfaceForm.value)
  
  if (!surfaceForm.value.surfaceId || !surfaceForm.value.startDate || !surfaceForm.value.endDate || !surfaceForm.value.price) {
    console.error('=== Ошибка валидации ===')
    console.error('Не все поля заполнены')
    alert('Пожалуйста, заполните все поля')
    return
  }

  try {
    console.log('=== Отправляем запрос на добавление поверхности ===')
    console.log('Данные для отправки:', {
      surfaceId: parseInt(surfaceForm.value.surfaceId),
      startDate: surfaceForm.value.startDate,
      endDate: surfaceForm.value.endDate,
      price: parseFloat(surfaceForm.value.price),
      priceType: surfaceForm.value.priceType
    })
    
    await addContractItem(parseInt(contractId), {
      surfaceId: parseInt(surfaceForm.value.surfaceId),
      startDate: surfaceForm.value.startDate,
      endDate: surfaceForm.value.endDate,
      price: parseFloat(surfaceForm.value.price),
      priceType: surfaceForm.value.priceType
    })
    
    console.log('=== Поверхность успешно добавлена ===')
    
    // Обновляем данные договора
    console.log('=== Обновляем данные договора ===')
    await getContract(contractId)
    
    // Сбрасываем форму
    surfaceForm.value = {
      surfaceId: '',
      startDate: '',
      endDate: '',
      price: 0,
      priceType: 'PerShow'
    }
    
    console.log('=== Форма сброшена ===')
  } catch (err) {
    console.error('=== Ошибка при добавлении поверхности ===')
    console.error('Ошибка:', err)
    console.error('Тип ошибки:', err?.constructor?.name)
    console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
    
    const errorMessage = err instanceof Error ? err.message : 'Ошибка добавления поверхности'
    alert(errorMessage)
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
  return `${surface.address}, ${surface.construction.name}`
}

const handleDeleteContract = async () => {
  console.log('=== Обрабатываем удаление договора ===')
  console.log('Contract value:', contract.value)
  
  if (!contract.value) {
    console.error('=== Ошибка: договор не найден ===')
    alert('Договор не найден')
    return
  }
  
  console.log(`=== ID договора для удаления: ${contract.value.id} ===`)
  const confirmed = confirm(`Вы уверены, что хотите удалить договор №${contract.value.id}?`)
  if (!confirmed) {
    console.log('=== Удаление отменено пользователем ===')
    return
  }
  
  try {
    console.log('=== Отправляем запрос на удаление договора ===')
    await deleteContract(contract.value.id)
    console.log('=== Договор успешно удален ===')
    // Перенаправляем на список договоров
    console.log('=== Перенаправляем на список договоров ===')
    router.push('/contracts')
  } catch (err) {
    console.error('=== Ошибка при удалении договора ===')
    console.error('Ошибка:', err)
    console.error('Тип ошибки:', err?.constructor?.name)
    console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
    
    const errorMessage = err instanceof Error ? err.message : 'Ошибка удаления договора'
    alert(errorMessage)
  }
}

// Функции для форматирования
const formatDate = (dateString) => {
  if (!dateString) return ''
  try {
    const date = new Date(dateString)
    return date.toLocaleDateString('ru-RU')
  } catch {
    return String(dateString)
  }
}

const formatPrice = (price) => {
  if (!price && price !== 0) return '0 ₽'
  return new Intl.NumberFormat('ru-RU', {
    style: 'currency',
    currency: 'RUB',
    minimumFractionDigits: 2
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