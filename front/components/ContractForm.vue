<template>
  <form @submit.prevent="handleSubmit" class="contract-form">
    <div class="form-group">
      <label for="client">Клиент</label>
      <select 
        id="client" 
        v-model="formData.clientId" 
        required
        class="form-control"
      >
        <option value="">Выберите клиента</option>
        <option 
          v-for="client in clients" 
          :key="client.id" 
          :value="client.id"
        >
          {{ client.name }} ({{ client.phone }})
        </option>
      </select>
    </div>

    <div class="form-actions">
      <button type="submit" class="btn btn-primary" :disabled="loading">
        {{ submitText }}
      </button>
      <slot name="cancel">
        <NuxtLink to="/contracts" class="btn btn-secondary">
          Отмена
        </NuxtLink>
      </slot>
    </div>
  </form>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useClients } from '~/composable/useClients'
import { useContracts } from '~/composable/useContracts'

const props = defineProps({
  contract: {
    type: Object,
    default: null
  },
  submitText: {
    type: String,
    default: 'Создать договор'
  }
})

const { clients, loading: clientsLoading, error: clientsError, fetchClients } = useClients()
const { createContract, updateContract, loading: contractLoading } = useContracts()

const formData = ref({
  clientId: ''
})

const loading = ref(false)
const error = ref(null)

onMounted(async () => {
  console.log('=== Монтируем ContractForm ===')
  console.log('Props contract:', props.contract)
  try {
    console.log('=== Начинаем загрузку клиентов ===')
    await fetchClients()
    console.log('=== Клиенты успешно загружены ===')
    console.log('Клиенты:', clients.value)
    
    if (props.contract) {
      console.log('=== Устанавливаем данные для редактирования ===')
      formData.value.clientId = props.contract.clientId
      console.log('ClientId из props:', props.contract.clientId)
    }
  } catch (err) {
    console.error('=== Ошибка при загрузке клиентов ===')
    console.error('Ошибка:', err)
    console.error('Тип ошибки:', err?.constructor?.name)
    console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
  }
})

const handleSubmit = async () => {
  console.log('=== Обрабатываем отправку формы ===')
  console.log('FormData:', formData.value)
  console.log('Props contract:', props.contract)
  
  if (!formData.value.clientId) {
    console.error('=== Ошибка валидации ===')
    console.error('Не выбран клиент')
    error.value = 'Пожалуйста, выберите клиента'
    return
  }

  loading.value = true
  error.value = null

  try {
    console.log('=== Начинаем сохранение договора ===')
    if (props.contract) {
      console.log('=== Обновляем существующий договор ===')
      console.log(`ID договора: ${props.contract.id}`)
      console.log('Данные для обновления:', {
        status: props.contract.status
      })
      await updateContract(props.contract.id, {
        status: props.contract.status
      })
    } else {
      console.log('=== Создаем новый договор ===')
      console.log('Данные для создания:', {
        clientId: parseInt(formData.value.clientId)
      })
      await createContract({
        clientId: parseInt(formData.value.clientId)
      })
    }
    
    console.log('=== Договор успешно сохранен ===')
    emit('submit')
  } catch (err) {
    console.error('=== Ошибка при сохранении договора ===')
    console.error('Ошибка:', err)
    console.error('Тип ошибки:', err?.constructor?.name)
    console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
    
    const errorMessage = err instanceof Error ? err.message : 'Ошибка сохранения договора'
    error.value = errorMessage
  } finally {
    loading.value = false
  }
}

const emit = defineEmits(['submit'])
</script>

<style scoped>
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