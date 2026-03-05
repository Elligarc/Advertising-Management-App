import { ref, computed } from 'vue'

// Типы данных на основе Swagger
export interface CityResponseModel {
  id: number
  name: string | null
}

export interface DistrictResponseModel {
  id: number
  name: string | null
}

export interface FormatResponseModel {
  id: number
  name: string | null
  constructionType: 'Billboard' | 'Poster' | 'CityLight' | 'Videoboard'
}

export interface ConstructionResponseModel {
  id: number
  address: string | null
  district: DistrictResponseModel
  format: FormatResponseModel
  city: CityResponseModel
}

export interface SurfaceResponseModel {
  id: number
  side: 'A' | 'B' | 'C'
  surfaceType: 'Digital' | 'Regular'
  loopDuration: number | null
  slotDuration: number | null
  maxSlots: number
  currentPrice: number
  currentPriceType: 'PerShow' | 'PerMonth'
  currentStatus: 'Created' | 'Decommissioned' | 'UnderRepair'
  construction: ConstructionResponseModel
}

export interface CreateSurfaceData {
  constructionId: number
  side: 'A' | 'B' | 'C'
  surfaceType: 'Digital' | 'Regular'
  loopDuration?: number | null
  slotDuration?: number | null
  initialPrice: number
  priceType: 'PerShow' | 'PerMonth'
}

export interface UpdateSurfaceData {
  loopDuration?: number | null
  slotDuration?: number | null
}

export interface UpdateSurfacePriceData {
  price: number
  priceType: 'PerShow' | 'PerMonth'
  dateFrom?: string | null
}

export interface UpdateSurfaceStatusData {
  status: 'Created' | 'Decommissioned' | 'UnderRepair'
  dateFrom: string
}

export interface SurfaceFilterParams {
  CityId?: number
  DistrictId?: number
  FormatId?: number
  ConstructionType?: 'Billboard' | 'Poster' | 'CityLight' | 'Videoboard'
  PriceType?: 'PerShow' | 'PerMonth'
  AvailableDaysFrom?: string
  AvailableDaysTo?: string
  AvailableHours?: boolean[]
  AvailableMonths?: number[]
}

export function useSurfaces() {
  const apiBase = useRuntimeConfig().public.apiBase
  const surfaces = ref<SurfaceResponseModel[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  // Получение всех поверхностей
  const fetchSurfaces = async () => {
    loading.value = true
    error.value = null
    try {
      const data = await $fetch<SurfaceResponseModel[]>(`${apiBase}/api/Surfaces/filter`)
      surfaces.value = data
    } catch (err) {
      console.error('Error fetching surfaces:', err)
      error.value = 'Ошибка загрузки поверхностей'
      surfaces.value = []
    } finally {
      loading.value = false
    }
  }

  // Получение поверхности по ID
  const fetchSurfaceById = async (id: number): Promise<SurfaceResponseModel | null> => {
    try {
      const data = await $fetch<SurfaceResponseModel>(`${apiBase}/api/Surfaces/${id}`)
      return data
    } catch (err) {
      console.error('Error fetching surface:', err)
      error.value = 'Ошибка загрузки поверхности'
      return null
    }
  }

  // Фильтрация поверхностей
  const filterSurfaces = async (params: SurfaceFilterParams): Promise<SurfaceResponseModel[]> => {
    try {
      const data = await $fetch<SurfaceResponseModel[]>(`${apiBase}/api/Surfaces/filter`, {
        params
      })
      return data
    } catch (err) {
      console.error('Error filtering surfaces:', err)
      error.value = 'Ошибка фильтрации поверхностей'
      return []
    }
  }

  // Создание новой поверхности
  const createSurface = async (data: CreateSurfaceData): Promise<SurfaceResponseModel | null> => {
    try {
      const result = await $fetch<SurfaceResponseModel>(`${apiBase}/api/Surfaces`, {
        method: 'POST',
        body: data
      })
      // Добавляем новую поверхность в список
      surfaces.value.push(result)
      return result
    } catch (err) {
      console.error('Error creating surface:', err)
      error.value = 'Ошибка создания поверхности'
      return null
    }
  }

  // Обновление поверхности
  const updateSurface = async (id: number, data: UpdateSurfaceData): Promise<SurfaceResponseModel | null> => {
    try {
      const result = await $fetch<SurfaceResponseModel>(`${apiBase}/api/Surfaces/${id}`, {
        method: 'PUT',
        body: data
      })
      // Обновляем поверхность в списке
      const index = surfaces.value.findIndex(s => s.id === id)
      if (index !== -1) {
        surfaces.value[index] = result
      }
      return result
    } catch (err) {
      console.error('Error updating surface:', err)
      error.value = 'Ошибка обновления поверхности'
      return null
    }
  }

  // Удаление поверхности
  const deleteSurface = async (id: number): Promise<boolean> => {
    try {
      await $fetch(`${apiBase}/api/Surfaces/${id}`, {
        method: 'DELETE'
      })
      // Удаляем поверхность из списка
      surfaces.value = surfaces.value.filter(s => s.id !== id)
      return true
    } catch (err) {
      console.error('Error deleting surface:', err)
      error.value = 'Ошибка удаления поверхности'
      return false
    }
  }

  // Обновление цены поверхности
  const updateSurfacePrice = async (id: number, data: UpdateSurfacePriceData): Promise<SurfaceResponseModel | null> => {
    try {
      const result = await $fetch<SurfaceResponseModel>(`${apiBase}/api/Surfaces/${id}/price`, {
        method: 'PUT',
        body: data
      })
      // Обновляем поверхность в списке
      const index = surfaces.value.findIndex(s => s.id === id)
      if (index !== -1) {
        surfaces.value[index] = result
      }
      return result
    } catch (err) {
      console.error('Error updating surface price:', err)
      error.value = 'Ошибка обновления цены поверхности'
      return null
    }
  }

  // Обновление статуса поверхности
  const updateSurfaceStatus = async (id: number, data: UpdateSurfaceStatusData): Promise<SurfaceResponseModel | null> => {
    try {
      const result = await $fetch<SurfaceResponseModel>(`${apiBase}/api/Surfaces/${id}/status`, {
        method: 'PUT',
        body: data
      })
      // Обновляем поверхность в списке
      const index = surfaces.value.findIndex(s => s.id === id)
      if (index !== -1) {
        surfaces.value[index] = result
      }
      return result
    } catch (err) {
      console.error('Error updating surface status:', err)
      error.value = 'Ошибка обновления статуса поверхности'
      return null
    }
  }

  // Вычисляемые свойства для удобства использования
  const surfaceCount = computed(() => surfaces.value.length)
  const hasSurfaces = computed(() => surfaces.value.length > 0)

  return {
    surfaces,
    loading,
    error,
    fetchSurfaces,
    fetchSurfaceById,
    filterSurfaces,
    createSurface,
    updateSurface,
    deleteSurface,
    updateSurfacePrice,
    updateSurfaceStatus,
    surfaceCount,
    hasSurfaces
  }
}