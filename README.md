# Mind TV

## Python development

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

Start EEG stream using EmotivPro application.  Use headset sim script if a dummy headset is needed:
```
python ./headset_sim.py
```

To start back end manually:
```
<start streaming eeg over LSL>
python ./main.py
```

If backend won't exit properly, kill the job
```
(ctrl + Z)
kill %1
```



