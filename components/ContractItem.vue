<template>
  <div class="contract-item">
    <div class="item-info">
      <div class="surface-details">
        <h4>{{ getSurfaceLabel(surface) }}</h4>
        <p class="construction-address">{{ surface.construction.address }}</p>
      </div>
      <div class="item-dates">
        <span class="date-label">С:</span>
        <span>{{ formatDate(startDate) }}</span>
        <span class="date-label">По:</span>
        <span>{{ formatDate(endDate) }}</span>
      </div>
    </div>
    <div class="item-footer">
      <span class="price">
        Цена: {{ formatPrice(price) }} ({{ getPriceTypeText(priceType) }})
      </span>
      <span class="total-price">
        Сумма: {{ formatPrice(totalPrice) }}
      </span>
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  surface: {
    type: Object,
    required: true
  },
  startDate: {
    type: String,
    required: true
  },
  endDate: {
    type: String,
    required: true
  },
  price: {
    type: Number,
    required: true
  },
  priceType: {
    type: String,
    required: true
  },
  totalPrice: {
    type: Number,
    required: true
  }
})

const getSurfaceLabel = (surface) => {
  return `${surface.construction.address}, ${surface.side} сторона, ${surface.surfaceType}`
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

const getPriceTypeText = (priceType) => {
  return priceType === 'PerShow' ? 'За показ' : 'За месяц'
}
</script>

<style scoped>
.contract-item {
  border: 1px solid #e0e0e0;
  border-radius: 6px;
  padding: 12px;
  background: #f8f9fa;
  margin-bottom: 12px;
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
</style>