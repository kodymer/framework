<template>
  <header>
    <div class="__under-construction">
      <p>// TODO: HeaderComponent</p>
      <nav>
        <RouterLink :to="{ name: 'home' }">Home</RouterLink>
        &nbsp;
        <RouterLink :to="{ name: 'login' }">Login</RouterLink>
        &nbsp;
        <RouterLink :to="{ name: 'insured-search' }">Insured search</RouterLink>
        &nbsp;
        <RouterLink :to="{ name: 'new-case' }">New case</RouterLink>
      </nav>
    </div>
  </header>
</template>
