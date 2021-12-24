<template>  
  <div id="app">
    <div class="flex-container">
      <header>
        <Header />
      </header>

      <section class="content">        
          <Main msg="Welcome to Your Vue.js App"/>
      </section>

      <footer>
         <Footer />
      </footer>
    </div>        
  </div>
</template>

<script>
import Main from './components/Main.vue'
import Header from './components/Header.vue'
import Footer from './components/Footer.vue'


export default {
  name: 'App',
  components: {
    Main,
    Header,
    Footer
  }
}
</script>

