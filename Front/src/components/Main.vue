<template>
<div class="ui container">  

  <sui-grid :columns="2">
    <sui-grid-row>
      <sui-grid-column  style="padding-right:0;width:50%">
        <sui-dropdown       
          size="small" 
            multiple     
            :options="skills"
            placeholder="All"
            search
            selection
            v-model="current"
            style="width:100%"
          />
      </sui-grid-column>
      <sui-grid-column style="padding-left:0;;width:50%">
        <sui-input width="10px" placeholder="Поиск..." icon="search" size="small" style="height:38px;width:100%"  /> 
      </sui-grid-column>
    </sui-grid-row>
  </sui-grid>



  <sui-grid celled>
    <GameListItem
      v-for="post in posts"
      v-bind:key="post.id"
      v-bind:post="post">
    </GameListItem>
  </sui-grid>



  <p>
    <sui-button color="green" @click="say('tiny.nes')">Запустить</sui-button>
  </p>

  <div id="gamediv" style="margin-top:0.5rem">
  </div>
</div>  
</template>

<script>
import GameListItem from './GameListItem.vue'

export default {
  name: 'Main',  
  components: { 
    GameListItem 
  },
  props: {
    msg: String
  },
  data() {
    return {
      posts: [
        { id: 1, title: 'My journey with Vue', genre: 'action' },
        { id: 2, title: 'Blogging with Vue', genre: 'action' },
        { id: 3, title: 'Why Vue is so fun', genre: 'action' }
      ],
      current: 'angular',
      skills: [
        { key: 'NES', text: 'NES', value: 'NES' },
        { key: 'SNES', text: 'SNES', value: 'SNES' },
        { key: 'SMS', text: 'Sega Master System', value: 'SMS' },
        { key: 'SMD', text: 'Sega MegaDrive', value: 'SMD' },
        { key: 'GB', text: 'GameBoy', value: 'GB' },
        { key: 'GBA', text: 'GameBoy Advance', value: 'GBA' },
        { key: 'TBG', text: 'TurboGraphics-16', value: 'TBG' },
        { key: 'ember', text: 'Ember', value: 'ember' },
        { key: 'html', text: 'HTML', value: 'html' },
        { key: 'ia', text: 'Information Architecture', value: 'ia' },
        { key: 'javascript', text: 'Javascript', value: 'javascript' },
        { key: 'mech', text: 'Mechanical Engineering', value: 'mech' },
        { key: 'meteor', text: 'Meteor', value: 'meteor' },
        { key: 'node', text: 'NodeJS', value: 'node' },
        { key: 'plumbing', text: 'Plumbing', value: 'plumbing' },
        { key: 'python', text: 'Python', value: 'python' },
        { key: 'rails', text: 'Rails', value: 'rails' },
        { key: 'react', text: 'React', value: 'react' },
        { key: 'repair', text: 'Kitchen Repair', value: 'repair' },
        { key: 'ruby', text: 'Ruby', value: 'ruby' },
        { key: 'ui', text: 'UI Design', value: 'ui' },
        { key: 'ux', text: 'User Experience', value: 'ux' },
      ]
    }
  },
  methods: {
    say(gameName) {    
      var findgameloader = document.getElementById("gameloader");
      if (findgameloader !== null) { // Prevents empty list item.
        return;
      } 

      var newgamediv = document.createElement("div"); // Create li element.
      newgamediv.setAttribute("id", "gameloader");
      newgamediv.style = "width:640px;height:480px;max-width:100%;";     
      var b = document.createElement('div');
      b.id = 'game';
      newgamediv.appendChild(b);
      
      var scriptnes = document.createElement('script');
      scriptnes.innerHTML = "EJS_player = '#game'; EJS_gameName = '" + gameName + "'; EJS_biosUrl = ''; EJS_gameUrl = 'roms/" + gameName + "'; EJS_core = 'nes'; EJS_pathtodata = 'data/'; ";
      document.body.appendChild(scriptnes);
      var scriptnes2 = document.createElement('script');
      scriptnes2.src = 'data/loader.js';
      document.body.appendChild(scriptnes2);
      
      var findgamediv = document.getElementById("gamediv");
      findgamediv.appendChild(newgamediv);

      var clickplay = setInterval(() => {
        if (typeof document.getElementsByClassName('ejs--73f9b4e94a7a1fe74e11107d5ab2ef')[0] !== 'undefined') {
          clearInterval(clickplay);
          document.getElementsByClassName('ejs--73f9b4e94a7a1fe74e11107d5ab2ef')[0].click();
        }
      }, 100);	  

    }
  }
}
</script>
