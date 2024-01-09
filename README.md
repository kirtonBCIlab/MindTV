# Mind TV

## Python setup

Use a conda environment to set up development tooling:
```
cd <repo>/MindTV-Python/
conda env create -f ./environment.yml
conda activate mindtv
```

Install dependencies with pip, use -e for development (local changes picked up without reinstalling)
```
pip install -e .
```

To start back end:
```
<start streaming eeg over LSL>
python ./main.py
```

If backend won't exit properly, kill the job
```
(ctrl + Z)
kill %1
```




