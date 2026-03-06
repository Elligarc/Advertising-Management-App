import { ref } from 'vue'
import type { 
  ContractResponseModel, 
  ContractStatus, 
  CreateContractData, 
  CreateContractItemData, 
  PriceType, 
  UpdateContractData 
} from '~/types/contracts'

export function useContracts() {
  const apiBase = useRuntimeConfig().public.apiBase
  const contracts = ref<ContractResponseModel[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const getContracts = async (): Promise<ContractResponseModel[]> => {
    loading.value = true
    error.value = null
    try {
      console.log('=== Загружаем список договоров ===')
      console.log('URL запроса:', `${apiBase}/api/contracts`)
      
      const response = await $fetch<ContractResponseModel[]>(`${apiBase}/api/contracts`)
      
      console.log('=== Список договоров успешно загружен ===')
      console.log('Ответ сервера:', response)
      console.log('Количество договоров:', response.length)
      
      contracts.value = response
      return response
    } catch (err) {
      console.error('=== Ошибка при загрузке списка договоров ===')
      console.error('Ошибка:', err)
      console.error('Тип ошибки:', err?.constructor?.name)
      console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
      
      const errorMessage = err instanceof Error ? err.message : 'Ошибка загрузки договоров'
      error.value = errorMessage
      console.error('Ошибка загрузки договоров:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const getContract = async (id: number): Promise<ContractResponseModel> => {
    loading.value = true
    error.value = null
    try {
      console.log(`Запрос договора с ID: ${id}`)
      console.log(`URL запроса: ${apiBase}/api/contracts/${id}`)
      
      const response = await $fetch<ContractResponseModel>(`${apiBase}/api/contracts/${id}`)
      
      console.log('Получен ответ от сервера:', response)
      console.log('Тип ответа:', typeof response)
      console.log('ID в ответе:', response?.id)
      
      // Добавляем договор в массив contracts для корректной работы вычисляемого свойства
      const existingIndex = contracts.value.findIndex(contract => contract.id === id)
      if (existingIndex !== -1) {
        console.log(`Обновляем существующий договор с ID: ${id}`)
        contracts.value[existingIndex] = response
      } else {
        console.log(`Добавляем новый договор с ID: ${id}`)
        contracts.value.push(response)
      }
      
      console.log('Текущий массив contracts:', contracts.value)
      
      return response
    } catch (err) {
      console.error('Ошибка при загрузке договора:', err)
      console.error('Тип ошибки:', err?.constructor?.name)
      console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
      
      const errorMessage = err instanceof Error ? err.message : 'Ошибка загрузки договора'
      error.value = errorMessage
      throw err
    } finally {
      loading.value = false
    }
  }

  const createContract = async (data: CreateContractData): Promise<ContractResponseModel> => {
    loading.value = true
    error.value = null
    try {
      console.log('=== Создаем новый договор ===')
      console.log('Данные для создания:', data)
      console.log('URL запроса:', `${apiBase}/api/contracts`)
      
      const response = await $fetch<ContractResponseModel>(`${apiBase}/api/contracts`, {
        method: 'POST',
        body: data
      })
      
      console.log('=== Договор успешно создан ===')
      console.log('Ответ сервера:', response)
      
      contracts.value.push(response)
      return response
    } catch (err) {
      console.error('=== Ошибка при создании договора ===')
      console.error('Ошибка:', err)
      console.error('Тип ошибки:', err?.constructor?.name)
      console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
      
      const errorMessage = err instanceof Error ? err.message : 'Ошибка создания договора'
      error.value = errorMessage
      console.error('Ошибка создания договора:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const updateContract = async (id: number, data: UpdateContractData): Promise<ContractResponseModel> => {
    loading.value = true
    error.value = null
    try {
      console.log('=== Обновляем договор ===')
      console.log(`ID договора: ${id}`)
      console.log('Данные для обновления:', data)
      console.log('URL запроса:', `${apiBase}/api/contracts/${id}`)
      
      const response = await $fetch<ContractResponseModel>(`${apiBase}/api/contracts/${id}`, {
        method: 'PUT',
        body: data
      })
      
      console.log('=== Договор успешно обновлен ===')
      console.log('Ответ сервера:', response)
      
      // Update the contract in the local array
      const index = contracts.value.findIndex(contract => contract.id === id)
      if (index !== -1) {
        contracts.value[index] = response
        console.log(`Договор с ID ${id} обновлен в локальном массиве`)
      }
      
      return response
    } catch (err) {
      console.error('=== Ошибка при обновлении договора ===')
      console.error('Ошибка:', err)
      console.error('Тип ошибки:', err?.constructor?.name)
      console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
      
      const errorMessage = err instanceof Error ? err.message : 'Ошибка обновления договора'
      error.value = errorMessage
      console.error('Ошибка обновления договора:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const addContractItem = async (
    contractId: number, 
    data: CreateContractItemData
  ): Promise<any> => {
    loading.value = true
    error.value = null
    try {
      console.log('=== Добавляем поверхность к договору ===')
      console.log(`ID договора: ${contractId}`)
      console.log('Данные для добавления:', data)
      console.log('URL запроса:', `${apiBase}/api/contracts/${contractId}/items`)
      
      const response = await $fetch(`${apiBase}/api/contracts/${contractId}/items`, {
        method: 'POST',
        body: data
      })
      
      console.log('=== Поверхность успешно добавлена ===')
      console.log('Ответ сервера:', response)
      
      // Refresh the contract to get updated items
      console.log('=== Обновляем данные договора ===')
      await getContract(contractId)
      
      return response
    } catch (err) {
      console.error('=== Ошибка при добавлении поверхности к договору ===')
      console.error('Ошибка:', err)
      console.error('Тип ошибки:', err?.constructor?.name)
      console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
      
      const errorMessage = err instanceof Error ? err.message : 'Ошибка добавления поверхности к договору'
      error.value = errorMessage
      console.error('Ошибка добавления поверхности к договору:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const deleteContract = async (id: number): Promise<void> => {
    loading.value = true
    error.value = null
    try {
      console.log('=== Удаляем договор ===')
      console.log(`ID договора: ${id}`)
      console.log('URL запроса:', `${apiBase}/api/contracts/${id}`)
      
      await $fetch(`${apiBase}/api/contracts/${id}`, {
        method: 'DELETE'
      })
      
      console.log('=== Договор успешно удален ===')
      
      // Remove the contract from the local array
      const index = contracts.value.findIndex(contract => contract.id === id)
      if (index !== -1) {
        console.log(`Удаляем договор с ID ${id} из локального массива`)
        contracts.value.splice(index, 1)
        console.log('Текущий массив contracts:', contracts.value)
      }
    } catch (err) {
      console.error('=== Ошибка при удалении договора ===')
      console.error('Ошибка:', err)
      console.error('Тип ошибки:', err?.constructor?.name)
      console.error('Сообщение об ошибке:', err instanceof Error ? err.message : String(err))
      
      const errorMessage = err instanceof Error ? err.message : 'Ошибка удаления договора'
      error.value = errorMessage
      console.error('Ошибка удаления договора:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    contracts,
    loading,
    error,
    getContracts,
    getContract,
    createContract,
    updateContract,
    addContractItem,
    deleteContract
  }
}