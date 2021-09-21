import JSEncrypt from 'jsencrypt';
import { ref, Ref } from 'vue';
import { ElMessage } from 'element-plus';
import { AxiosResponse } from 'axios';
import { LandingData, GetFlushKeyResponse } from '@/api/modules/user';
import useApi from '@/api';

export type UseLanding = {
  form: Ref<LandingData>,
  loading: Ref<boolean>,
  handleSubmit: () => boolean,
}

export const useLanding = (
  submitMethod: 'login' | 'register', callback: () => void,
): UseLanding => {
  const form = ref<LandingData>({
    userName: '',
    passwordHash: '',
    connectId: '',
  });
  const loading = ref<boolean>(false);
  const Apis = useApi();
  const handleSubmit = (): boolean => {
    if (loading.value) return false;
    if (!form.value.userName || !form.value.passwordHash) {
      ElMessage.error('用户名和密码为必填项！');
      return false;
    }
    loading.value = true;
    Apis.user.getFlushKey().then((res: AxiosResponse<GetFlushKeyResponse>) => {
      console.log('getFlushKey: ', res);
      const jse = new JSEncrypt();
      jse.setPublicKey(res.data.entity.key);
      Apis.user[submitMethod]({
        userName: form.value.userName,
        connectId: res.data.entity.identity,
        passwordHash: jse.encrypt(form.value.passwordHash) as string,
      }).then(callback).finally(() => {
        loading.value = false;
      });
    });
    return true;
  };
  return {
    form, loading, handleSubmit,
  };
};
