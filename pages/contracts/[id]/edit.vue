<template>
  <div class="edit-contract-page">
    <div class="page-header">
      <h1>Редактировать договор №{{ contract.id }}</h1>
    </div>

    <div v-if="loading" class="loading">
      Загрузка...
    </div>

    <div v-if="error" class="error">
      {{ error }}
    </div>

    <ContractForm 
      v-if="contract" 
      :contract="contract" 
      submit-text="Сохранить изменения"
      @submit="handleFormSubmit"
    >
      <template #cancel>
        <NuxtLink :to="`/contracts/${contract.id}`" class="btn btn-secondary">
          Отмена
        </NuxtLink>
      </template>
    </ContractForm>
  </div>
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRoute } from 'vue-router'
import { useContracts } from '~/composable/useContracts'
import ContractForm from '~/components/ContractForm.vue'

const route = useRoute()
const contractId = route.params.id

const { contracts, loading, error, getContract } = useContracts()

const contract = computed(() => {
  return contracts.value.find(c => c.id === parseInt(contractId))
})

const loadingForm = ref(false)
const errorForm = ref(null)

onMounted(async () => {
  await getContract(contractId)
})

const handleFormSubmit = () => {
  // После успешного сохранения перенаправляем на страницу деталей
  window.location.href = `/contracts/${contractId}`
}
</script>

<style scoped>
.edit-contract-page {
  padding: 20px;
}

.page-header {
  margin-bottom: 20px;
}

.page-header h1 {
  margin: 0;
  color: #333;
}
</style>