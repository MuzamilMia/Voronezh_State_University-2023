#include<windows.h>
#include<thread>
#include<mutex>
#include<queue>

struct Pair
{
	size_t left, right;
};

class ThreadSafeQueue
{
private:
	std::mutex mutex;
	std::queue<Pair> queue;
public:
	ThreadSafeQueue(){}
	void push(Pair elem)
	{
		queue.push(elem);
	}
	bool try_pop(Pair& elem)
	{
		bool result{ false };
		if (!queue.empty())
		{
			std::lock_guard<std::mutex>locer(mutex);
			if (!queue.empty())
			{
				result = true;
				elem = queue.front();
				queue.pop();
			}
		}
		return result;
	}
	bool empty()
	{
		return queue.empty();
	}

};

const size_t COUNT = 110;
const size_t NUM_THREADS = 4;
ThreadSafeQueue TSQ;
std::mutex mutex_task;

void sum(int* arr, long volatile& global_sum)
{
	Pair pair{};
	while (TSQ.try_pop(pair))
	{
		long local_sum{};
		for (size_t i{ pair.left }; i< pair.right; ++i)
		{
			local_sum += arr[i];
			std::this_thread::sleep_for(std::chrono::microseconds(1));
		}
		std::lock_guard<std::mutex>locer(mutex_task);
		global_sum += local_sum;
		std::cout<< "id: " << std::this_thread::get_id() << ": " << local_sum << '\n';
	}
}

int sum_parallel(int* arr)
{
	
	size_t chunk{ 10 };
	size_t left{}, right{};
	while (right != COUNT)
	{
		left = right;
		right = right + chunk < COUNT ? right + chunk : COUNT;
		TSQ.push(Pair{ left,right });

	}
	std::thread thr[NUM_THREADS - 1];
	long volatile global_sum{};

	for (size_t i{}; i < NUM_THREADS - 1; ++i)
	{
		thr[i] = std::thread(sum, arr, std::ref(global_sum));
	}
	sum(arr, global_sum);
	for (int i{}; i < NUM_THREADS - 1; ++i)
		thr[i].join();

	return global_sum;
}

void fill(int* arr)
{
	//we are fillling the array here
}


int main()
{

	//we are calling here to the functions that we have made and there we are giving the 
	//parameters to the functions. 
	return 0;
}