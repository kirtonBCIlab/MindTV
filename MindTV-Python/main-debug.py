from bci_essentials.io.lsl_sources import LslMarkerSource
from bci_essentials.io.lsl_messenger import LslMessenger
from bci_essentials.eeg_data import EegData
from bci_essentials.classification.mi_classifier import MiClassifier

from MindTV.EmotivEegSource import EmotivEegSource

from bci_essentials.utils.logger import Logger

# Create a logger for this script
# Note: This is separate from the bci_essentials logger
logger = Logger(name="MindTV-Python")
logger.setLevel(Logger.DEBUG)  # Changed from defualt of INFO

bessy_logger = Logger()
bessy_logger.setLevel(Logger.DEBUG)

# create LSL sources, these will block until the outlets are present
logger.info("Creating LSL sources")
logger.debug("These will block until the outlets are present")
eeg_source = EmotivEegSource()
marker_source = LslMarkerSource()
messenger = LslMessenger()

# Select a classifier
logger.debug("Selecting MiClassifier()")
classifier = MiClassifier()  # you can add a subset here

# Set settings
logger.debug("Setting classifier settigs")
classifier.set_mi_classifier_settings(n_splits=3, type="TS", random_seed=35)

# Define the MI data object
logger.debug("Defining the MI data object")
mi_data = EegData(classifier, eeg_source, marker_source, messenger)

# Run
logger.debug("Setting online to True and training to True")
mi_data.setup(online=True, training=True)
logger.debug("Starting run() method")
mi_data.run()
