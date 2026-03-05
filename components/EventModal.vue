<template>
  <div v-if="show" class="modal-overlay" @click.self="$emit('close')">
    <div class="modal-content">
      <h3>{{ isEditMode ? 'Редактирование события' : 'События на ' + selectedDate }}</h3>
      
      <!-- Список событий -->
      <div v-if="!isEditMode" class="events-list">
        <div v-if="events.length === 0" class="no-events">
          Нет событий на этот день
        </div>
        <div v-for="event in events" :key="event.id" class="modal-event">
          <div class="event-info">
            <span :class="['event-badge', event.type]"></span>
            <span class="event-title">{{ event.title }}</span>
          </div>
          <div class="event-actions">
            <button class="icon-btn edit" @click="$emit('edit-event', event)" title="Редактировать">✏️</button>
            <button class="icon-btn delete" @click="$emit('delete-event', event.id)" title="Удалить">🗑️</button>
          </div>
        </div>
      </div>

      <!-- Форма добавления/редактирования -->
      <div v-else class="event-form">
        <div class="form-group">
          <label>Тип события:</label>
          <select v-model="formData.type">
            <option value="start">Начало аренды</option>
            <option value="end">Окончание аренды</option>
            <option value="repair">Ремонт/простой</option>
          </select>
        </div>
        <div class="form-group">
          <label>Дата:</label>
          <input type="date" v-model="formData.date" />
        </div>
        <div class="form-group">
          <label>Название:</label>
          <input type="text" v-model="formData.title" placeholder="Введите название события" />
        </div>
        <div class="form-actions">
          <button class="btn-primary" @click="saveEvent">
            {{ editingEvent ? 'Сохранить' : 'Добавить' }}
          </button>
          <button class="btn-secondary" @click="cancelEdit">Отмена</button>
        </div>
      </div>

      <!-- Кнопки действий -->
      <div v-if="!isEditMode" class="modal-actions">
        <button class="btn-primary" @click="startAddEvent">+ Добавить событие</button>
        <button class="btn-secondary" @click="$emit('close')">Закрыть</button>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'

const props = defineProps({
  show: Boolean,
  selectedDate: String,
  events: Array,
  isEditMode: Boolean,
  editingEvent: Object
})

const emit = defineEmits(['close', 'add-event', 'edit-event', 'delete-event', 'save-edit'])

const formData = ref({
  type: 'start',
  date: '',
  title: ''
})

// Инициализация формы при редактировании
watch(() => props.editingEvent, (newEvent) => {
  if (newEvent) {
    formData.value = { ...newEvent }
  } else {
    resetForm()
  }
}, { immediate: true })

function resetForm() {
  formData.value = {
    type: 'start',
    date: formatDateForInput(new Date()),
    title: ''
  }
}

function formatDateForInput(date) {
  const d = new Date(date)
  const year = d.getFullYear()
  const month = String(d.getMonth() + 1).padStart(2, '0')
  const day = String(d.getDate()).padStart(2, '0')
  return `${year}-${month}-${day}`
}

function startAddEvent() {
  resetForm()
  emit('edit-event', null)
}

function saveEvent() {
  if (!formData.value.title.trim()) {
    alert('Пожалуйста, введите название события')
    return
  }
  
  if (props.editingEvent) {
    emit('save-edit', { ...props.editingEvent, ...formData.value })
  } else {
    emit('add-event', formData.value)
  }
}

function cancelEdit() {
  if (props.editingEvent) {
    emit('edit-event', null)
  } else {
    emit('close')
  }
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0,0,0,0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
  animation: fadeIn 0.2s;
}

.modal-content {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  min-width: 500px;
  max-width: 600px;
  max-height: 80vh;
  overflow-y: auto;
  animation: slideUp 0.3s;
}

.modal-content h3 {
  margin-top: 0;
  margin-bottom: 1.5rem;
  color: #2c3e50;
}

.events-list {
  margin-bottom: 1.5rem;
  max-height: 400px;
  overflow-y: auto;
}

.modal-event {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.75rem;
  border: 1px solid #e9ecef;
  border-radius: 8px;
  margin-bottom: 0.5rem;
  background: #f8f9fa;
}

.modal-event:hover {
  background: #e9ecef;
}

.event-info {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  flex: 1;
}

.event-badge {
  width: 12px;
  height: 12px;
  border-radius: 3px;
}

.event-badge.start { background: #d4edda; }
.event-badge.end { background: #f8d7da; }
.event-badge.repair { background: #fff3cd; }

.event-title {
  font-size: 0.9rem;
  color: #2c3e50;
}

.event-actions {
  display: flex;
  gap: 0.5rem;
}

.icon-btn {
  background: none;
  border: none;
  cursor: pointer;
  font-size: 1.2rem;
  padding: 0.25rem;
  border-radius: 4px;
  transition: background 0.2s;
}

.icon-btn.edit:hover {
  background: #e3f2fd;
}

.icon-btn.delete:hover {
  background: #ffebee;
}

.no-events {
  text-align: center;
  padding: 2rem;
  color: #7f8c8d;
  font-style: italic;
}

.event-form {
  margin-bottom: 1.5rem;
}

.form-group {
  margin-bottom: 1rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #2c3e50;
  font-weight: 500;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #e9ecef;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.2s;
}

.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #2a5298;
}

.form-actions,
.modal-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
}

.btn-primary {
  padding: 0.75rem 1.5rem;
  background: #2a5298;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-primary:hover {
  background: #1a3a6f;
}

.btn-secondary {
  padding: 0.75rem 1.5rem;
  background: #e9ecef;
  color: #2c3e50;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  transition: background 0.2s;
}

.btn-secondary:hover {
  background: #dee2e6;
}

@keyframes fadeIn {
  from { opacity: 0; }
  to { opacity: 1; }
}

@keyframes slideUp {
  from { transform: translateY(20px); opacity: 0; }
  to { transform: translateY(0); opacity: 1; }
}
</style>