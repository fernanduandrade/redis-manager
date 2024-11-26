<script setup lang="ts">
import redis from './api/redis'
import { Connection } from './common/domain';
import { ref } from 'vue'
import ConnectionForm from './common/components/ConnectionForm/index.vue'


const showConnectionFormModal = ref(false)
function closeFormEvent(evt: boolean) {
  showConnectionFormModal.value = evt
}
const connections = ref<Array<Connection>>([
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_1' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_2' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_3' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_4' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_5' },
  { host: 'localhost', port: '6339', password: '', username: 'localhost', name: 'teste_6' }
])

const textValueExample = ref('')

// TODO componentizar para resolver problema de keyspaces que tem filhos
// TODO usar data tree data structure
</script>

<template>
  <ConnectionForm :visible="showConnectionFormModal" @close-form="closeFormEvent" />
  <main class="relative min-h-screen flex">
    <div class="bg-[#F9FAFE] w-[300px] p-4 flex flex-col items-center gap-5 border-r-2 border-red-gray">
      <Button @click="showConnectionFormModal = true">Adicionar nova conexão</Button>
      <section>
        <div class="card flex flex-col gap-2">
          <Panel :header="`${connection.host}:${connection.port}`" v-for="(connection, index) in connections"
            :key="index" toggleable :collapsed="true">
            
          </Panel>
        </div>
      </section>
    </div>

    <div class="flex-1 flex justify-center items-center p-4 text-2xl bg-[#F9FAFE]">
      <p class="text-[100px] font-semibold">se liga aqui mané</p>
      <br/>
      <p class="text-[100px] font-semibold">{{ textValueExample }}</p>
    </div>
  </main>

</template>
