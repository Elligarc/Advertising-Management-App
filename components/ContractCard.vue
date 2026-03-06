<template>
  <div v-if="isValidContract" class="contract-card">
    <div class="contract-header">
      <div class="contract-info">
        <h3>Договор №{{ contract.id }}</h3>
        <p class="client-name">{{ contract.clientName || 'Не указан' }}</p>
        <div class="contract-dates">
          <span class="date-label">С:</span>
          <span>{{ formatDate(contract.startDate) }}</span>
          <span class="date-label">По:</span>
          <span>{{ formatDate(contract.endDate) }}</span>
        </div>
      </div>
      <div class="contract-actions">
        <slot name="actions">
          <NuxtLink :to="`/contracts/${contract.id}`" class="btn btn-secondary">
            Детали
          </NuxtLink>
        </slot>
      </div>
    </div>
    <div class="contract-footer">
      <span class="status" :class="contract.status.toLowerCase()">
        {{ getStatusText(contract.status) }}
      </span>
      <span class="total-price">
        Сумма: {{ formatPrice(contract.totalPrice) }}
      </span>
    </div>
  </div>
  <div v-else class="contract-card error-card">
    <div class="error-message">
      Некорректные данные договора
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  contract: {
    type: Object,
    required: true
  }
})

// Валидация данных контракта
const isValidContract = computed(() => {
  console.log('=== Проверяем валидность договора ===')
  console.log('Props contract:', props.contract)
  
  const isValid = props.contract && 
         typeof props.contract.id === 'number' &&
         typeof props.contract.status === 'string' &&
         typeof props.contract.totalPrice === 'number'
  
  console.log('=== Результат валидации ===', isValid)
  return isValid
})

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
.contract-card {
  background: white;
  border-radius: 8px;
  padding: 16px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid #e0e0e0;
}

.contract-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 12px;
}

.contract-info h3 {
  margin: 0 0 8px 0;
  color: #333;
}

.client-name {
  margin: 0 0 8px 0;
  color: #666;
  font-size: 14px;
}

.contract-dates {
  display: flex;
  gap: 8px;
  font-size: 12px;
  color: #888;
}

.date-label {
  font-weight: bold;
}

.contract-actions {
  display: flex;
  gap: 8px;
}

.contract-footer {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding-top: 12px;
  border-top: 1px solid #eee;
}

.status {
  padding: 4px 8px;
  border-radius: 4px;
  font-size: 12px;
  font-weight: bold;
  text-transform: uppercase;
}

.error-card {
  border: 2px solid #dc3545;
  background-color: #f8d7da;
}

.error-message {
  color: #721c24;
  font-weight: bold;
  text-align: center;
  padding: 20px;
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
}
</style>