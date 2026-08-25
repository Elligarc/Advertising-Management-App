export type ContractStatus = 'Created' | 'Active' | 'Cancelled'

export type PriceType = 'PerShow' | 'PerMonth'

export interface SurfaceBriefResponseModel {
  id: number
  name: string
  address: string
  cityId: number
  districtId: number
  formatId: number
  constructionId: number
  price: number
  status: string
  loopDuration: number | null
  latitude: number | null
  longitude: number | null
  photoUrl: string | null
  city: {
    id: number
    name: string
  }
  district: {
    id: number
    name: string
  }
  format: {
    id: number
    name: string
  }
  construction: {
    id: number
    name: string
  }
}

export interface ContractItemResponseModel {
  id: number
  surfaceId: number
  startDate: string
  endDate: string
  price: number
  priceType: PriceType
  totalPrice: number
  surface: SurfaceBriefResponseModel
}

export interface ContractResponseModel {
  id: number
  clientId: number
  startDate: string | null
  endDate: string | null
  totalPrice: number
  status: ContractStatus
  clientName: string | null
  items: ContractItemResponseModel[] | null
}

export interface CreateContractData {
  clientId: number
}

export interface UpdateContractData {
  status: ContractStatus
}

export interface CreateContractItemData {
  surfaceId: number
  startDate: string        // ISO date-time
  endDate: string          // ISO date-time
  price: number
  priceType: PriceType
  daysOfWeek?: number[]    // nullable
  hoursInDay?: number[]    // nullable
}