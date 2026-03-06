<template>
  <div class="add-contract-page">
    <div class="page-header">
      <h1>Создать договор</h1>
    </div>

    <div v-if="loading" class="loading">
      Создание договора...
    </div>

    <div v-if="error" class="error">
      {{ error }}
    </div>

    <ContractForm 
      submit-text="Создать договор"
      @submit="handleFormSubmit"
    >
      <template #cancel>
        <NuxtLink to="/contracts" class="btn btn-secondary">
          Отмена
        </NuxtLink>
      </template>
    </ContractForm>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useClients } from '~/composable/useClients'
import { useContracts } from '~/composable/useContracts'
import { useRouter } from 'vue-router'
import ContractForm from '~/components/ContractForm.vue'

const router = useRouter()
const { clients, clientsLoading, clientsError, fetchClients } = useClients()
const { createContract: createContractApi, loading: contractLoading, error: contractError } = useContracts()

const loading = ref(false)
const error = ref(null)
const formData = ref({
  clientId: ''
})

onMounted(async () => {
  await fetchClients()
})

const handleFormSubmit = async () => {
  console.log('=== Обрабатываем событие submit из ContractForm ===')
  console.log('=== Начинаем создание договора ===')
  console.log('FormData:', formData.value)

  if (!formData.value.clientId) {
    console.error('=== Ошибка валидации ===')
    console.error('Не выбран клиент')
    error.value = 'Пожалуйста, выберите клиента'
    return
  }

  loading.value = true
  error.value = null

  try {
    console.log('=== Отправляем запрос на создание договора ===')
    console.log('Данные для создания:', {
      clientId: parseInt(formData.value.clientId)
    })
    const createdContract = await createContractApi({
      clientId: parseInt(formData.value.clientId)
    })
    console.log('=== Договор успешно создан ===')
    console.log('ID созданного договора:', createdContract.id)

    // Перенаправляем на детальную страницу созданного договора
    console.log('=== Перенаправляем на детальную страницу договора ===')
    router.push(`/contracts/${createdContract.id}`)
  } catch (err) {
    console.error('=== Ошибка при создании договора ===')
    console.error('Ошибка:', err)
    console.error('Тип ошибки:', err?.constructor?.name)
    console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))

    const errorMessage = err instanceof Error ? err.message : 'Ошибка создания договора'
    error.value = errorMessage
  } finally {
    loading.value = false
  }
}
</script>
<style scoped>
.add-contract-page {
  padding: 20px;
}

.page-header {
  margin-bottom: 20px;
}

.page-header h1 {
  margin: 0;
  color: #333;
}

.contract-form {
  max-width: 600px;
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  border: 1px solid #e0e0e0;
}

.form-group {
  margin-bottom: 20px;
}

.form-group label {
  display: block;
  margin-bottom: 8px;
  font-weight: 500;
  color: #333;
}

.form-control {
  width: 100%;
  padding: 10px;
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
  gap: 12px;
  justify-content: flex-end;
}

.btn {
  padding: 10px 20px;
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

.btn-secondary {
  background-color: #6c757d;
  color: white;
  border-color: #6c757d;
}

.btn-secondary:hover {
  background-color: #545b62;
  border-color: #545b62;
}

.loading, .error {
  padding: 10px;
  margin-bottom: 20px;
  border-radius: 4px;
}

.loading {
  background-color: #e9ecef;
  color: #495057;
  text-align: center;
}

.error {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}
</style>