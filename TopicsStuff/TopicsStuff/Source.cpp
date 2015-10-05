
__global__ void reverse(int *in, int *out) {
	int threadsPerBlock = 256;
	__shared__ int cache[threadsPerBlock];
	int tid = threadIdx.x + blockIdx.x * blockDim.x;
	int cacheIndex = threadIdx.x;

	int temp = 0;
	temp = in[tid];
	out[tid] = temp;

	__syncthreads();
}