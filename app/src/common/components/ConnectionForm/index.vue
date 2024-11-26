<script lang="ts" setup>
import { reactive, ref } from 'vue';
import { Connection } from '../../domain';

const emit = defineEmits(['closeForm'])

const props = defineProps({
  visible: {
    type: Boolean
  }
})

const connection = reactive<Connection>({
    host: '',
    password: '',
    port: '',
    username: '',
    name: ''
})

const loadConnection = ref(false)

function addConnection() {
  loadConnection.value = true
  loadConnection.value = false
}

function hideForm() {
  emit('closeForm', false)
}
// TODO salvar no localStorage as conexões com sucesso
</script>

<template>
    <Dialog v-model:visible="props.visible" modal header="Nova conexão" :style="{ width: '660px' }" @update:visible="hideForm">
    <main class="flex flex-col gap-4">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">

        <div class="flex flex-col gap-1">
          <InputText v-model="connection.host" placeholder="Host" />
        </div>

        <div class="flex flex-col gap-1">
          <InputText v-model="connection.port" placeholder="6379" />
        </div>

        <div class="flex flex-col gap-1">
          <InputText v-model="connection.username" placeholder="Username" />
        </div>

        <div class="flex flex-col gap-1">
          <InputText v-model="connection.password" placeholder="Password" />
        </div>

      </div>
      <div class="flex items-center justify-center w-full">
        <Button :loading="loadConnection" class="w-[200px]" label="Conectar" @click="addConnection"/>
      </div>

    </main>

  </Dialog>
</template>