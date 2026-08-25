<template>
  <div class="contracts-page">
    <div class="page-header">
      <h1>Договоры</h1>
      <NuxtLink to="/contracts/add" class="btn btn-primary">
        Создать договор
      </NuxtLink>
    </div>

    <div v-if="loading" class="loading">
      Загрузка договоров...
    </div>

    <div v-if="error" class="error">
      {{ error }}
    </div>

    <div v-if="!loading && !error && contracts.length === 0" class="empty-state">
      Договоров пока нет
    </div>

    <div v-else-if="contracts.length > 0" class="contracts-list">
      <ContractCard 
        v-for="contract in contracts" 
        :key="contract.id" 
        :contract="contract"
      />
    </div>
  </div>
</template>

<script setup>
import { useContracts } from '~/composable/useContracts'
import ContractCard from '~/components/ContractCard.vue'

const { contracts, loading, error, getContracts } = useContracts()

onMounted(async () => {
  console.log('=== Монтируем список договоров ===')
  try {
    console.log('=== Загружаем список договоров ===')
    await getContracts()
    console.log('=== Список договоров успешно загружен ===')
    console.log('Количество договоров:', contracts.value?.length || 0)
  } catch (err) {
    console.error('=== Ошибка при загрузке списка договоров ===')
    console.error('Ошибка:', err)
    console.error('Тип ошибки:', err?.constructor?.name)
    console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
  }
})
</script>
<style scoped>
.contracts-page {
  padding: 20px;
}

.page-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.page-header h1 {
  margin: 0;
  color: #333;
}

.contracts-list {
  display: grid;
  gap: 16px;
}

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

.loading, .error, .empty-state {
  text-align: center;
  padding: 40px;
  color: #666;
}

.error {
  color: #dc3545;
}

.btn {
  display: inline-block;
  padding: 8px 16px;
  border-radius: 4px;
  text-decoration: none;
  font-weight: 500;
  border: 1px solid transparent;
  cursor: pointer;
  transition: all 0.2s;
}

.btn-primary {
  background-color: #007bff;
  color: white;
  border-color: #007bff;
}

.btn-primary:hover {
  background-color: #0056b3;
  border-color: #0056b3;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
  border-color: #6c757d;
}

.btn-secondary:hover {
  background-color: #545b62;
  border-color: #545b62;
}
</style>