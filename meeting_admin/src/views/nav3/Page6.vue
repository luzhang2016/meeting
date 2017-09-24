<template>
	<section>page6...
	<div id="Demo">
    <p v-if="main"> Main </p>
    <p v-if="family"> A </p>
    <p v-if="age>25">Age:{{age}} </p>
	<p v-if="name.indexOf('lin')>0">Name:{{name}}</p>
</div>
	</section>

</template>

<script>

  export default {
     data(){
		 return{
			main:true,
			family:false,
			age:28,
			name:'Colin'
		}
     }
  }
</script>
