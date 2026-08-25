<template>
  <div class="surface-list">
    <table class="surface-table">
      <thead>
        <tr>
          <th>Адрес</th>
          <th>Тип</th>
          <th>Формат</th>
          <th>Сторона</th>
          <th>Статус</th>
          <th>Цена</th>
          <th>Действия</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="surface in surfaces" :key="surface.id" :class="rowClass(surface.currentStatus)">
          <td>
            <div class="address-cell">
              <span class="address">{{ surface.construction?.address }}</span>
              <span class="city">{{ surface.construction?.city?.name }}</span>
            </div>
          </td>
          <td>
            <span class="type-badge">{{ surface.construction?.format?.constructionType }}</span>
          </td>
          <td>
            <span class="format">{{ surface.construction?.format?.name }}</span>
          </td>
          <td>
            <span class="side">{{ surface.side }}</span>
          </td>
          <td>
            <span :class="['status', statusClass(surface.currentStatus)]">{{ statusText(surface.currentStatus) }}</span>
          </td>
          <td>
            <div class="price-cell">
              <span class="price">{{ surface.currentPrice }} ₽</span>
              <span class="price-type">{{ surface.currentPriceType === 'PerMonth' ? '/ мес' : '/ показ' }}</span>
            </div>
          </td>
          <td>
            <div class="actions">
              <NuxtLink :to="`/surfaces/${surface.id}`" class="btn btn-primary btn-sm">Подробнее</NuxtLink>
              <NuxtLink :to="`/surfaces/${surface.id}/edit`" class="btn btn-secondary btn-sm">Редактировать</NuxtLink>
              <button class="btn btn-secondary btn-sm" type="button" @click="$emit('add-to-contract', surface.id)">
                Добавить в договор
              </button>
              <button 
                class="btn btn-success btn-sm" 
                :disabled="surface.currentStatus !== 'Created'" 
                @click="$emit('rent-surface', surface.id)"
              >
                Сдать в аренду
              </button>
            </div>
          </td>
        </tr>
      </tbody>
    </table>
    
    <div v-if="surfaces.length === 0" class="empty-state">
      Поверхности не найдены
    </div>
  </div>
</template>

<script setup>
import { defineProps, defineEmits } from 'vue'

const props = defineProps({
  surfaces: {
    type: Array,
    required: true
  }
})

const emit = defineEmits(['add-to-contract', 'rent-surface'])

function statusClass(status) {
  return { Created: 'status-free', UnderRepair: 'status-repair', Decommissioned: 'status-busy' }[status] ?? ''
}

function statusText(status) {
  return { Created: 'Активна', UnderRepair: 'На ремонте', Decommissioned: 'Выведена' }[status] ?? status
}

function rowClass(status) {
  return { 
    'row-active': status === 'Created',
    'row-repair': status === 'UnderRepair',
    'row-decommissioned': status === 'Decommissioned'
  }
}
</script>

<style scoped>
.surface-list {
  width: 100%;
  background: white;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  overflow: hidden;
}

.surface-table {
  width: 100%;
  border-collapse: collapse;
  font-size: 0.9rem;
}

.surface-table th {
  background: #f8fafc;
  color: #374151;
  font-weight: 600;
  text-align: left;
  padding: 1rem;
  font-size: 0.85rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  border-bottom: 2px solid #e5e7eb;
}

.surface-table td {
  padding: 1rem;
  border-bottom: 1px solid #f3f4f6;
  vertical-align: middle;
}

.row-active:hover {
  background-color: #f0f9ff;
}

.row-repair:hover {
  background-color: #fffbeb;
}

.row-decommissioned:hover {
  background-color: #fee2e2;
}

.address-cell {
  display: flex;
  flex-direction: column;
  gap: 0.25rem;
}

.address {
  font-weight: 600;
  color: #111827;
  font-size: 0.95rem;
}

.city {
  font-size: 0.75rem;
  color: #6b7280;
  text-transform: uppercase;
}

.type-badge {
  background: #eef2ff;
  color: #4f46e5;
  padding: 0.25rem 0.5rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
}

.format {
  font-weight: 500;
  color: #374151;
  font-size: 0.85rem;
}

.side {
  font-weight: 600;
  color: #1f2937;
  background: #f3f4f6;
  padding: 0.25rem 0.5rem;
  border-radius: 6px;
  font-size: 0.8rem;
}

.price-cell {
  display: flex;
  flex-direction: column;
  gap: 0.125rem;
}

.price {
  font-weight: 700;
  font-size: 1rem;
  color: #111827;
}

.price-type {
  font-size: 0.75rem;
  color: #6b7280;
}

.status {
  padding: 0.25rem 0.75rem;
  border-radius: 999px;
  font-size: 0.75rem;
  font-weight: 600;
  text-transform: uppercase;
}

.status-free {
  background: #dcfce7;
  color: #166534;
}

.status-repair {
  background: #fef3c7;
  color: #92400e;
}

.status-busy {
  background: #fee2e2;
  color: #991b1b;
}

.actions {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
}

.btn-sm {
  padding: 0.5rem 0.75rem;
  font-size: 0.75rem;
  border-radius: 6px;
  border: 1px solid transparent;
  cursor: pointer;
  transition: all 0.2s;
  font-weight: 500;
}

.btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.btn-primary {
  background: #2563eb;
  color: white;
  border-color: #2563eb;
}

.btn-primary:hover:not(:disabled) {
  background: #1d4ed8;
  border-color: #1d4ed8;
}

.btn-secondary {
  background: #f3f4f6;
  color: #374151;
  border-color: #e5e7eb;
}

.btn-secondary:hover {
  background: #e5e7eb;
  border-color: #d1d5db;
}

.btn-success {
  background: #10b981;
  color: white;
  border-color: #10b981;
}

.btn-success:hover:not(:disabled) {
  background: #059669;
  border-color: #059669;
}

.empty-state {
  padding: 2rem;
  text-align: center;
  color: #6b7280;
  font-size: 0.9rem;
  border-top: 1px solid #f3f4f6;
}

/* Responsive design */
@media (max-width: 768px) {
  .surface-table {
    font-size: 0.8rem;
  }
  
  .surface-table th,
  .surface-table td {
    padding: 0.75rem 0.5rem;
  }
  
  .actions {
    flex-direction: column;
    gap: 0.25rem;
  }
  
  .btn-sm {
    padding: 0.4rem 0.6rem;
    font-size: 0.7rem;
  }
}
</style>