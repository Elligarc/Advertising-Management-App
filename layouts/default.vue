<!-- layouts/default.vue -->
<template>
  <div class="app">
    <header class="header">
      <div class="logo">
        <NuxtLink to="/" class="logo-link">
          <h1>Surface Manager</h1>
        </NuxtLink>
      </div>
      <nav class="nav">
        <NuxtLink to="/" class="nav-link" active-class="active">Главная</NuxtLink>
        <NuxtLink to="/surfaces" class="nav-link" active-class="active">Поверхности</NuxtLink>
        <NuxtLink to="/clients" class="nav-link" active-class="active">Клиенты</NuxtLink>
        <NuxtLink to="/contracts" class="nav-link" active-class="active">Договоры</NuxtLink>
        <NuxtLink to="/downtime" class="nav-link" active-class="active">Простои</NuxtLink>
        <NuxtLink to="/calendar" class="nav-link" active-class="active">Календарь</NuxtLink>
      </nav>
      <div class="user">
        <span class="user-name">Менеджер</span>
        <span class="avatar" aria-hidden="true"></span>
      </div>
    </header>

    <main class="main">
      <slot />
    </main>

    <footer class="footer">
      <p>© 2026 Surface Manager — Автоматизация рекламного бизнеса</p>
    </footer>
  </div>
</template>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  font-family: system-ui, -apple-system, 'Segoe UI', Roboto, sans-serif;
}

body {
  background-color: #f8fafc;
  color: #1a1a2e;
}

.app {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

.logo-link {
  color: inherit;
  text-decoration: none;
}

.header {
  background: #1a1a2e;
  color: #fff;
  padding: 0.875rem 1.5rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 1px 3px rgba(0,0,0,0.12);
}

.logo h1 {
  font-size: 1.35rem;
  font-weight: 600;
  letter-spacing: -0.02em;
}

.nav {
  display: flex;
  gap: 0.25rem;
}

.nav-link {
  color: rgba(255,255,255,0.75);
  text-decoration: none;
  padding: 0.5rem 0.875rem;
  border-radius: 6px;
  font-size: 0.9375rem;
  font-weight: 500;
  transition: color 0.2s, background 0.2s;
}

.nav-link:hover {
  color: #fff;
  background: rgba(255,255,255,0.08);
}

.nav-link.active {
  color: #fff;
  background: rgba(255,255,255,0.12);
}

.user {
  display: flex;
  align-items: center;
  gap: 0.75rem;
  padding: 0.375rem 0.75rem;
  border-radius: 6px;
  font-size: 0.875rem;
  color: rgba(255,255,255,0.9);
}

.avatar {
  width: 28px;
  height: 28px;
  border-radius: 50%;
  background: rgba(255,255,255,0.25);
}

.main {
  flex: 1;
  padding: 1.5rem 1.5rem 2rem;
  max-width: 1280px;
  margin: 0 auto;
  width: 100%;
}

.footer {
  background: #1a1a2e;
  color: rgba(255,255,255,0.7);
  text-align: center;
  padding: 1rem 1.5rem;
  margin-top: auto;
  font-size: 0.875rem;
}

.card {
  background: #fff;
  border-radius: 10px;
  padding: 1.25rem 1.5rem;
  box-shadow: 0 1px 3px rgba(0,0,0,0.06);
  border: 1px solid #e5e7eb;
  transition: box-shadow 0.2s;
}

.card:hover {
  box-shadow: 0 4px 12px rgba(0,0,0,0.08);
}

.btn {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  transition: all 0.3s;
  font-weight: 500;
  text-decoration: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
}

.btn-primary {
  background: #1a1a2e;
  color: #fff;
}

.btn-primary:hover {
  background: #2d2d44;
  box-shadow: 0 2px 8px rgba(26,26,46,0.25);
}

.btn-success {
  background: linear-gradient(135deg, #28a745, #34ce57);
  color: white;
}

.btn-secondary {
  background: #e5e7eb;
  color: #374151;
}
.btn-secondary:hover {
  background: #d1d5db;
}

.btn-warning {
  background: linear-gradient(135deg, #ffc107, #ffdb6d);
  color: #333;
}

.btn-danger {
  background: linear-gradient(135deg, #dc3545, #ff6b7c);
  color: white;
}

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

.grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 1.5rem;
  margin-top: 2rem;
}

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
  border-color: #1a1a2e;
}

.select-filter {
  padding: 0.75rem;
  border: 2px solid #e0e0e0;
  border-radius: 8px;
  font-size: 1rem;
  min-width: 150px;
}
</style>
