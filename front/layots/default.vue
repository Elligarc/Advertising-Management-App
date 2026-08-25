<!-- layouts/default.vue -->
<template>
  <div class="app">
    <!-- Шапка сайта -->
    <header class="header">
      <div class="logo">
        <h1>Surface Manager</h1>
      </div>
      <nav class="nav">
        <NuxtLink to="/" class="nav-link" active-class="active">
          Главная
        </NuxtLink>
        <NuxtLink to="/surfaces" class="nav-link" active-class="active">
          Поверхности
        </NuxtLink>
        <NuxtLink to="/clients" class="nav-link" active-class="active">
          Клиенты
        </NuxtLink>
        <NuxtLink to="/downtime" class="nav-link" active-class="active">
          Простои
        </NuxtLink>
        <NuxtLink to="/calendar" class="nav-link" active-class="active">
          Календарь
        </NuxtLink>
      </nav>
      <div class="user">
        <span class="user-name">Менеджер</span>
        <span class="avatar" aria-hidden="true"></span>
      </div>
    </header>

    <!-- Основной контент -->
    <main class="main">
      <slot />
    </main>

    <!-- Подвал -->
    <footer class="footer">
      <p>© 2026 Surface Manager — Система управления рекламными поверхностями</p>
    </footer>
  </div>
</template>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
}

body {
  background-color: #f0f2f5;
}

.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

/* Шапка */
.header {
  background: linear-gradient(135deg, #1e3c72, #2a5298);
  color: white;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 4px 12px rgba(0,0,0,0.1);
}

.logo h1 {
  font-size: 1.5rem;
  font-weight: 500;
}

.nav {
  display: flex;
  gap: 1.5rem;
}

.nav-link {
  color: rgba(255,255,255,0.8);
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-radius: 8px;
  transition: all 0.3s;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.nav-link:hover {
  background-color: rgba(255,255,255,0.1);
  color: white;
}

.nav-link.active {
  background-color: rgba(255,255,255,0.2);
  color: white;
}

.icon {
  font-size: 1.2rem;
}

.user {
  display: flex;
  align-items: center;
  gap: 1rem;
  background-color: rgba(255,255,255,0.1);
  padding: 0.5rem 1rem;
  border-radius: 8px;
}

.avatar {
  font-size: 1.5rem;
}

/* Основной контент */
.main {
  flex: 1;
  padding: 2rem;
  max-width: 1400px;
  margin: 0 auto;
  width: 100%;
}

/* Подвал */
.footer {
  background-color: #2c3e50;
  color: white;
  text-align: center;
  padding: 1.5rem;
  margin-top: auto;
}

/* Общие стили для карточек */
.card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
  transition: transform 0.3s, box-shadow 0.3s;
}

.card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0,0,0,0.15);
}

/* Кнопки */
.btn {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s;
  font-weight: 500;
}

.btn-primary {
  background: linear-gradient(135deg, #1e3c72, #2a5298);
  color: white;
}

.btn-primary:hover {
  background: linear-gradient(135deg, #2a5298, #1e3c72);
  transform: translateY(-1px);
  box-shadow: 0 4px 12px rgba(42,82,152,0.3);
}

.btn-success {
  background: linear-gradient(135deg, #28a745, #34ce57);
  color: white;
}

.btn-warning {
  background: linear-gradient(135deg, #ffc107, #ffdb6d);
  color: #333;
}

.btn-danger {
  background: linear-gradient(135deg, #dc3545, #ff6b7c);
  color: white;
}

/* Статусы */
.status {
  padding: 0.25rem 0.75rem;
  border-radius: 20px;
  font-size: 0.875rem;
  font-weight: 600;
  display: inline-block;
}

.status-free {
  background-color: #d4edda;
  color: #155724;
  border: 1px solid #c3e6cb;
}

.status-busy {
  background-color: #f8d7da;
  color: #721c24;
  border: 1px solid #f5c6cb;
}

.status-repair {
  background-color: #fff3cd;
  color: #856404;
  border: 1px solid #ffeeba;
}

/* Сетка */
.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
  margin-top: 2rem;
}

/* Поиск и фильтры */
.filters {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  margin-bottom: 2rem;
  display: flex;
  gap: 1rem;
  flex-wrap: wrap;
  box-shadow: 0 2px 8px rgba(0,0,0,0.1);
}

.search-input {
  flex: 1;
  min-width: 250px;
  padding: 0.75rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
  transition: border-color 0.3s;
}

.search-input:focus {
  outline: none;
  border-color: #2a5298;
}

.select-filter {
  padding: 0.75rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
  min-width: 150px;
}
</style>