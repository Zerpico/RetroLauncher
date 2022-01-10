<template>
  <div id="app">
    <div class="flex-container">
      <header>
        <Header />
      </header>

      <section class="content">
        <div class="ui container">
          <router-view />
        </div>
      </section>

      <footer>
        <Footer />
      </footer>
    </div>
  </div>
</template>

<script>
import Header from "./components/static/Header.vue";
import Footer from "./components/static/Footer.vue";

export default {
  name: "App",
  components: {
    Header,
    Footer,
  },
};
</script>

<style>
#nav {
  padding: 30px;
}

#nav a {
  font-weight: bold;
  color: #2c3e50;
}

#nav a.router-link-exact-active {
  color: #42b983;
}
</style>
