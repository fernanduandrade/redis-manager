<script lang="ts" setup>
import { reactive, ref } from 'vue';
import { Connection } from '../../domain'
import redis from '../../../api/redis'
const emit = defineEmits(['closeForm', 'updateConnection'])

const props = defineProps({
  visible: {
    type: Boolean
  }
})

const connectionErrorMessages = ref('')

const connection = reactive<Connection>({
  host: '',
  password: '',
  port: '',
  username: '',
  name: '',
  open: false
})

const loadConnection = ref(false)

async function addConnection() {
  loadConnection.value = true
  if (!connection.port)
    connection.port = '6379'

  if (!connection.host)
    connection.host = 'localhost'

  const result = await redis.createConnection(connection)
  if (result === null) {
    connectionErrorMessages.value = "Não foi possível realizar a conexão, porfavor verificar os parametros"
    loadConnection.value = !loadConnection.value
    return
  }
  const connectionSubmitted: Connection = {
    id: result,
    host: connection.host,
    password: connection.password,
    port: connection.port,
    username: connection.username,
    name: connection.name,
    open: false
  }
  emit('updateConnection', connectionSubmitted)
  hideForm()
}

function hideForm() {
  emit('closeForm', false)
  loadConnection.value = false
  connection.host = ''
  connection.password = ''
  connection.username = ''
  connection.port = ''
  connection.name = ''
}
</script>

<template>
  <Dialog v-model:visible="props.visible" modal header="Nova conexão" :style="{ width: '660px' }"
    @update:visible="hideForm">
    <main class="flex flex-col gap-4">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">

        <div class="flex flex-col gap-1">
          <label for="host">Host *</label>
          <InputText id="host" v-model="connection.host" placeholder="localhost" />
        </div>

        <div class="flex flex-col gap-1">
          <label for="port">Port *</label>
          <InputText id="port" v-model="connection.port" placeholder="6379" />
        </div>

        <div class="flex flex-col gap-1">
          <label for="username">Username</label>
          <InputText id="username" v-model="connection.username" placeholder="Username" />
        </div>

        <div class="flex flex-col gap-1">
          <label for="password">Password</label>
          <InputText id="password" v-model="connection.password" placeholder="Password" />
        </div>

        <div class="flex flex-col gap-1">
          <label for="name">Name</label>
          <InputText id="name" v-model="connection.name" placeholder="Nome da conexão" />
        </div>

      </div>
      <div class="flex items-center justify-end w-full">
        <Button :loading="loadConnection" class="w-[200px]" label="Conectar" @click="addConnection" />
      </div>
      <span class="text-red-600" v-if="connectionErrorMessages">
        {{ connectionErrorMessages }}
      </span>

    </main>

  </Dialog>
</template>