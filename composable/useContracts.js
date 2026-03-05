import { ref } from 'vue'

export function useContracts() {
  const contracts = ref([])
  const loading = ref(false)
  const error = ref(null)

  const fetchContracts = async () => {
    loading.value = true
    error.value = null
    try {
      const response = await $fetch('/api/Contracts')
      contracts.value = response
    } catch (err) {
      error.value = err.message || 'Ошибка загрузки договоров'
      console.error('Ошибка загрузки договоров:', err)
    } finally {
      loading.value = false
    }
  }

  const createContract = async (contractData) => {
    loading.value = true
    error.value = null
    try {
      const response = await $fetch('/api/Contracts', {
        method: 'POST',
        body: contractData
      })
      contracts.value.push(response)
      return response
    } catch (err) {
      error.value = err.message || 'Ошибка создания договора'
      console.error('Ошибка создания договора:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const getContract = async (id) => {
    loading.value = true
    error.value = null
    try {
      const response = await $fetch(`/api/Contracts/${id}`)
      return response
    } catch (err) {
      error.value = err.message || 'Ошибка загрузки договора'
      console.error('Ошибка загрузки договора:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  const addSurfaceToContract = async (contractId, surfaceData) => {
    loading.value = true
    error.value = null
    try {
      const response = await $fetch(`/api/Contracts/${contractId}/items`, {
        method: 'POST',
        body: surfaceData
      })
      return response
    } catch (err) {
      error.value = err.message || 'Ошибка добавления поверхности к договору'
      console.error('Ошибка добавления поверхности к договору:', err)
      throw err
    } finally {
      loading.value = false
    }
  }

  return {
    contracts,
    loading,
    error,
    fetchContracts,
    createContract,
    getContract,
    addSurfaceToContract
  }
}