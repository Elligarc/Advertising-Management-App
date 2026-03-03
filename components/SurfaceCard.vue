<template>
  <div class="surface-card">
    <h3>{{ name }}</h3>
    <p>{{ address }}</p>
    <p>{{ price }} ₽/день</p>
    <p class="status" :class="statusClass">{{ statusText }}</p>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  name: String,
  address: String,
  price: Number,
  status: String, // 'free', 'busy', 'repair'
  statusText: String
})

const statusClass = computed(() => ({
  'status-free': props.status === 'free',
  'status-busy': props.status === 'busy',
  'status-repair': props.status === 'repair'
}))
</script>

<style scoped>
.surface-card {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 15px;
  margin: 10px;
  background: white;
}

.status {
  font-weight: bold;
  padding: 5px 10px;
  border-radius: 4px;
  display: inline-block;
}

.status-free {
  background-color: #28a745;
  color: white;
}

.status-busy {
  background-color: #dc3545;
  color: white;
}

.status-repair {
  background-color: #ffc107;
  color: black;
}
</style>