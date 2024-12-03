import { isEmpty } from 'ramda'
import { ref } from 'vue'

type HttpRequest = 'POST' | 'DELETE' | 'GET' | 'PUT' | 'PATCH'

interface Options {
  headers?: Record<string, string>
  method?: HttpRequest
  body?: string
}


export default function useFetch<TResult>(url: string, options: Options = {}) {
  const data = ref<TResult>()
  const error = ref('')

  const { signal, abort } = new AbortController()

  const fetchData = async() => {
    const defaultOption: Options = {
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      method: 'GET',
    }

    if (!isEmpty(options))
      options = { ...defaultOption, ...options }
    else
      options = { ...defaultOption }

    try {
      if (signal.aborted)
        return

      const response = await fetch(url, { signal, ...options })
      const result = await response.json()

      if(!response.ok) {
        error.value = result
      }

      data.value = result
    }
    catch (exception: any) {
      error.value = exception.message
    }
  }

  return { data, error, abort, fetchData }
}